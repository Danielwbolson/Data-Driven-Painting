﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SculptingVis {
    public class Glyph : VisualElement {


        static Glyph defaultGlyph;
		public static Glyph DefaultGlyph() {
			if(defaultGlyph == null) {
				Glyph glyph = CreateInstance<Glyph>();
                glyph._lodMeshes = new Mesh[1];
                glyph._lodMeshes[0] = ((SculptingVis.StyleController)FindObjectOfType(typeof(SculptingVis.StyleController))).sphere;
                glyph.SetName("Default");
                defaultGlyph = glyph;

			}
			return defaultGlyph;
		}  


        static Texture2D _defaultNormalMap;
        public static Texture2D DefaultNormalMap() {
            if (_defaultNormalMap == null) {
                _defaultNormalMap = new Texture2D(1, 1);
                _defaultNormalMap.SetPixel(0, 0, new Color(0.5f, 0.5f, 1.0f));
            }
            return _defaultNormalMap;
        }
        static Texture2D _defaultAlphaMap;

        public static Texture2D DefaulAlphaMap() {
            if (_defaultAlphaMap == null) {
                _defaultAlphaMap = new Texture2D(1, 1);
                _defaultAlphaMap.SetPixel(0, 0, new Color(1, 1, 1));
            }
            return _defaultAlphaMap;
        }
        [SerializeField]
        Texture2D _thumbnail;

        [SerializeField]
        Mesh[] _lodMeshes;

        [SerializeField]
        Dictionary<int, Texture2D> _normalMaps;

        [SerializeField]
        bool _providesNormalMap = false;

        public bool HasNormals() {
            return _providesNormalMap;
        }

        [SerializeField]
        Texture2D _alphaMap;

        [SerializeField]
        string _filePath;


        public Texture2D GetAlphaMap() {
            if (_alphaMap == null) {
                return DefaulAlphaMap();
            }
            return _alphaMap;
        }
        public int GetNumberOfLODs() {
            return GetLODMeshes().Length;
        }

         Mesh[] GetLODMeshes() {
             if(_lodMeshes == null) {
                 if (Path.GetExtension(_filePath).ToUpper() == ".GLYPH") {

                    // For now, name the glyph based on the file package name
                    string name = Path.GetFileNameWithoutExtension(_filePath);
                    SetName(name);

                    DirectoryInfo info = new DirectoryInfo(_filePath);
                    FileInfo[] fileInfo = info.GetFiles();
                    DirectoryInfo[] directoryInfo = info.GetDirectories();

                    // First let's iterate through the root of this package,
                    // looking for the OBJ files
                    foreach (var file in fileInfo) {

                        // An OBJ should contain the actual meshes of our glyph. 
                        // We could additionally handle individual OBJ's for 
                        // each LOD, but that can be added later.
                        if (file.Extension.ToUpper() == ".OBJ") {
                            if (Path.GetFileNameWithoutExtension(file.FullName).Contains("_LOD")) {
                                Debug.Log("Mesh Loading does not currently support individual LOD meshes.");

                            } else {
                                string objPath = file.FullName;
                                // Seth hacked todether the LoadOBJFileToMeshes function
                                // based on LoadOBJFile, just removing the Gameobject logic

                                Mesh[] meshes = OBJLoader.LoadOBJFileToMeshes(objPath);
                                _lodMeshes = new Mesh[meshes.Length];
                                for (int i = 0; i < meshes.Length; i++) {
                                    _lodMeshes[meshes.Length - 1 - i] = meshes[i];

                                }
                            }
                        }
                    }
                }
            }
            return _lodMeshes;
         }
        
        public Mesh GetLODMesh(int level) {
            level = Mathf.Min(GetLODMeshes().Length-1,level);
            return GetLODMeshes()[level];
        }

        public Texture2D GetLODNormalMap(int level) {
            if (_normalMaps != null && _normalMaps.ContainsKey(level)) return _normalMaps[level];
            return DefaultNormalMap();
        }

        public void SetFilePath(string f) {
            _filePath = f;
        }

        public static VisualElement LoadFile(string filePath) {

            // Check to see if this filePath points to a Glyph package
            if (Path.GetExtension(filePath).ToUpper() == ".GLYPH") {

                // Go ahead and create the empty glyph object
                Glyph glyph = CreateInstance<Glyph>();
                    string name = Path.GetFileNameWithoutExtension(filePath);
                    glyph.SetName(name);
                glyph._providesNormalMap = false;
                DirectoryInfo info = new DirectoryInfo(filePath);
                FileInfo[] fileInfo = info.GetFiles();
                DirectoryInfo[] directoryInfo = info.GetDirectories();

                // Now that we know how many meshes there are, we can set up our 
                // normal maps, and pre-populate them with a default bump map

                //if(glyph.GetNumberOfLODs() > 0){
                glyph._normalMaps = new Dictionary<int, Texture2D>();
                //}

                // Second, let's iterate through the root of this package
                // looking for thumbnail image and the possible LOD textures.
                foreach (var file in fileInfo) {


                    // Expect each LOD normal map to start with "LOD" followed by an integer. 
                    if (file.Extension.ToUpper() == ".PNG" && file.Name.ToUpper().StartsWith("LOD")) {
                        string number = Path.GetFileNameWithoutExtension(file.FullName).Substring(3);
                        int level = int.Parse(number);

                        // Load the normalMap in and stick it in the list in reverse order.
                        Texture2D normalMap = new Texture2D(1, 1);
                        normalMap.LoadImage(File.ReadAllBytes(file.FullName));
                        glyph._normalMaps[level] = normalMap;
                        glyph._providesNormalMap = true;

                    }
                    // The only PNG we expect on this level is the thumbnail...
                    else if (file.Extension.ToUpper() == ".PNG") {
                        Texture2D loadedImage = new Texture2D(1, 1);
                        loadedImage.LoadImage(File.ReadAllBytes(file.FullName));
                        glyph._thumbnail = loadedImage;
                    }

                }

                // The only sub directory we expect right now is the normal maps, 
                // but if we have need for more we can handle them here.
                foreach (var directory in directoryInfo) {
                    if (directory.Name.ToUpper() == "NORMALMAPS") {
                        DirectoryInfo normalMapDir = new DirectoryInfo(directory.FullName);

                        // For each file in the NormalMaps sub-directory...
                        foreach (var file in normalMapDir.GetFiles()) {
                            // Expect each LOD normal map to start with "LOD" followed by an integer. 
                            if (file.Extension.ToUpper() == ".PNG" && file.Name.ToUpper().StartsWith("LOD")) {
                                string number = Path.GetFileNameWithoutExtension(file.FullName).Substring(3);
                                int level = int.Parse(number);

                                // Load the normalMap in and stick it in the list in reverse order.
                                Texture2D normalMap = new Texture2D(1, 1);
                                normalMap.LoadImage(File.ReadAllBytes(file.FullName));
                                glyph._normalMaps[level] = normalMap;
                                glyph._providesNormalMap = true;


                            }
                        }
                    }

                    if (directory.Name.ToUpper() == "ALPHAMAPS") {
                        DirectoryInfo alphaMapDir = new DirectoryInfo(directory.FullName);

                        // For each file in the AlphaMaps sub-directory...
                        foreach (var file in alphaMapDir.GetFiles()) {

                            if (file.Extension.ToUpper() == ".PNG") {

                                Texture2D alphaMap = new Texture2D(1, 1);
                                alphaMap.LoadImage(File.ReadAllBytes(file.FullName));
                                glyph._alphaMap = alphaMap;

                            }
                        }
                    }
                }
                return glyph;
            }
            return null;
        }

        public override Texture2D GetPreviewImage() {
            return _thumbnail;
        }

        public override float GetPreviewImageAspectRatio() {
            return 1.0f;
        }


    }
}
