using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stroke : MonoBehaviour {

    [HideInInspector]
    public List<Vertex> vertex_list;
    private List<Vertex> from;
    private List<Vertex> to;

    [HideInInspector]
    public float buffer;

    private Material _mat;
    private Mesh _mesh;
    private MeshFilter _mf;
    private MeshRenderer _mr;

    private List<Vector3> verts;
    private List<int> tris;
    private List<Color> cols;

    private int _numSides = 32;
    private float _width = 0.05f;

    private Color _startCol;
    private Color _endCol;

    private float strokeLen;
    private bool lerp = false;
    private float fraction = 0;

    // Use this for initialization
    void Awake() {
        _startCol = new Color(0.7f, 0.7f, 0, 1f);
        _endCol = new Color(0.7f, 0, 0.7f, 1f);

        vertex_list = new List<Vertex>();
        from = new List<Vertex>();
        to = new List<Vertex>();

        verts = new List<Vector3>();
        tris = new List<int>();
        cols = new List<Color>();

        _mr = gameObject.AddComponent<MeshRenderer>();
        _mf = gameObject.AddComponent<MeshFilter>();
        _mesh = _mf.mesh;

        _mat = new Material(Shader.Find("Sprites/Default"));
        _mr.material = _mat;

        _mesh.subMeshCount = 1;
        strokeLen = 0;
    }

    void Update() {
        if (lerp) {
            if (fraction < 1) {
                for (int i = 0; i < vertex_list.Count; i++) {
                    Vertex v = new Vertex {
                        position = Vector3.Lerp(from[i].position, to[i].position, fraction),
                        orientation = Quaternion.Lerp(from[i].orientation, to[i].orientation, fraction)
                    };

                    vertex_list[i] = v;
                }
                Refresh();
                fraction += Time.deltaTime * 0.5f;
            } else {
                fraction = 0;
                lerp = false;
            }
        }
    }

    public void AddVertex(Vertex v) {
        if (vertex_list.Count > 0) {
            strokeLen += Vector3.Distance(vertex_list[vertex_list.Count - 1].position, v.position);
        } 
        vertex_list.Add(v);

        Refresh();
    }

    /*
     * Algorithm below to create brush stroke is attributed to:
     * https://github.com/ivlab/Paint3D
     * VR-based 3D painting tool developed in Unity jointly by the UMN and MCAD.
     * Developed by: 
     */

    void Refresh() {

        // Clear out mesh data every time we refresh
        verts.Clear();
        tris.Clear();
        cols.Clear();

        float theta = 360.0f / _numSides;

        Vector3[] poly = new Vector3[_numSides];

        Vector3 rad = new Vector3(_width, 0, 0);
        for (int i = 0; i < _numSides; i++) {
            Quaternion q = Quaternion.Euler(0, 0, theta * i);
            poly[i] = q * rad;
        }

        int count = vertex_list.Count;

        // first cross-section
        Vector3 center = vertex_list[0].position;
        Quaternion rot = vertex_list[0].orientation;
        for (int i = 0; i < _numSides; i++) {
            Vector3 offset = rot * poly[i];
            Vector3 v = offset + center;
            verts.Add(v);
            cols.Add(_startCol);
        }

        // rest of cross sections
        for (int i = 1; i < count; i++) {
            center = vertex_list[i].position;
            rot = vertex_list[i].orientation;

            // Calculate which cross section this is
            int n = i * _numSides;

            // Interpolate color based on position in vertex array
            // Will dynamically change as more vertices are added
            float span = i / (float)count;
            Color color = Color.Lerp(_startCol, _endCol, span);

            for (int j = 0; j < _numSides; j++) {
                // Rotate at origin and then move to location
                Vector3 offset = rot * poly[j];
                Vector3 v = offset + center;
                verts.Add(v);
                cols.Add(color);

                // tri 1
                tris.Add(n + j); //vertex of a side
                tris.Add(n + j - _numSides); // vertex of same side, one cross-section back
                tris.Add((j + 1) % _numSides + n); // next vertex of cross-section, will be first vertex if last vertex in cross-section

                // tri 1 backwards
                tris.Add((j + 1) % _numSides + n); // next vertex of cross-section, will be first vertex if last vertex in cross-section
                tris.Add(n + j - _numSides); // vertex of same side, one cross-section back
                tris.Add(n + j); //vertex of a side

                // tri 2
                tris.Add((j + 1) % _numSides + n);
                tris.Add(n + j - _numSides);
                tris.Add(n + (j + 1) % _numSides - _numSides);

                // tri 2 backwards
                tris.Add(n + (j + 1) % _numSides - _numSides);
                tris.Add(n + j - _numSides);
                tris.Add((j + 1) % _numSides + n);
            }
        }
        _mesh.SetVertices(verts);
        _mesh.SetColors(cols);
        _mesh.SetTriangles(tris, 0);
    }

    /*
     * User has drawn a stoke.
     * Now we want to determine, in the data, the closest interpolated
     * streamline to the users stroke.
     *   - Generate a streamline through through all vertex points in stroke
     *     - Interpolated grid of data:
     *       - Follow in points primary direction at small steps
     *       - Calculate new points primary direction through weighted, bi-linear
     *         sampling of K closest data-points
     *       - Go in new direction
     *       - Repeat until out of data
     *     - Use Hermite Curves for streamlines so that we maintain data integrity
     *       of points and directions
     *   - Use Drawing With The Flow algorithm to determine the most simlar streamline
     * Animate morphing from stroke to selected streamline
     */
    public void MorphToData(FakeData f) {
        // This is the current stoke from the user

        // Generate a streamline through all vertex points in stroke
        // Of same length as stroke
        List<Streamline> streamLines = new List<Streamline>();

        int increment = vertex_list.Count / Mathf.CeilToInt(vertex_list.Count / 20.0f);

        for (int i = 0; i < vertex_list.Count; i+=increment) {
            // Generate a streamline per vertex, given the data
            Streamline s = GenerateStreamline(vertex_list[i], f, vertex_list.Count);
            streamLines.Add(s);
        }

        // Calculate similarity for each streamline thru function from Drawing with the Flow
        float[] similarities = new float[streamLines.Count];
        for (int i = 0; i < streamLines.Count; i++) {
            similarities[i] = 0;
            for (int j = 0; j < vertex_list.Count; j++) {
                int cp = ClosestPoint(vertex_list[j].position, streamLines[i].positions);

                Vector3 d = vertex_list[j].position - streamLines[i].positions[cp].position;
                similarities[i] += 10 * Vector3.Dot(d, d) + 
                    1 * Vector3.Dot(
                        vertex_list[j].orientation * gameObject.transform.forward, 
                        streamLines[i].positions[cp].orientation * gameObject.transform.forward);
            }
        }

        // Determine closest Streamline
        float min = Mathf.Infinity;
        int index = -1;
        for (int i = 0; i < similarities.Length; i++) {
            if (similarities[i] < min) {
                index = i;
                min = similarities[i];
            }
        }

        // Streamlines[i] is our chosen streamline, we need to refresh to this streamline
        lerp = true;
        from = new List<Vertex>(vertex_list);
        to = new List<Vertex>(streamLines[index].positions);
    }

    int ClosestPoint(Vector3 p, List<Vertex> l) {
        int closest = -1;

        float min = Mathf.Infinity;

        for (int i = 0; i < l.Count; i++) {
            float dist = Vector3.Distance(p, l[i].position);

            // check for divide by 0
            if (float.IsNaN(dist)) continue;

            // 4 closest points in 4 quadrants around point
            if (dist < min) {
                min = dist;
                closest = i;
            }
        }
        return closest;
    }

    // First points, then curves
    Streamline GenerateStreamline(Vertex p, FakeData f, int length) {
        Streamline s = new Streamline();
        List<Vertex> positive = new List<Vertex>();
        List<Vertex> negative = new List<Vertex>();

        buffer = strokeLen / length;

        // Get all valid, interpolated data-points in POSITIVE direction of streamline
        Vertex dataVert = BilinearVertex(p.position, f);
        Vector3 dir = dataVert.orientation * gameObject.transform.forward;
        int l = 0;

        while (l < (length / 2.0f + 0.001f)) {
            positive.Add(dataVert);

            Vector3 pos = dataVert.position + buffer * dir;
            dataVert = BilinearVertex(pos, f);
            dir = dataVert.orientation * gameObject.transform.forward;

            l++;
        }

        // Get all valid, interpolated data-points in NEGATIVE direction of streamline
        dataVert = BilinearVertex(p.position, f);
        dir = -(dataVert.orientation * gameObject.transform.forward);
        l = 0;
        while (l < (length / 2.0f + 0.001f)) {
            negative.Add(dataVert);

            Vector3 pos = dataVert.position + buffer * dir;
            dataVert = BilinearVertex(pos, f);
            dir = -(dataVert.orientation * gameObject.transform.forward);

            l++;
        }

        // Get our list of vertices in order for streamline
        negative.RemoveAt(0);
        negative.Reverse();
        negative.AddRange(positive);
        s.positions = negative;

        return s;
    }

    Vertex BilinearVertex(Vector3 p, FakeData f) {
        // 4 closest points
        int[] cpIndex = new int[4];
        cpIndex = BilinearPoints(p, f.positions);

        float dist_0 = Vector3.Distance(p, f.positions[cpIndex[0]]);
        float dist_1 = Vector3.Distance(p, f.positions[cpIndex[1]]);
        float dist_2 = Vector3.Distance(p, f.positions[cpIndex[2]]);
        float dist_3 = Vector3.Distance(p, f.positions[cpIndex[3]]);
        float total = dist_0 + dist_1 + dist_2 + dist_3;

        Vector3 pDir = 
            (dist_0 / total) * f.primaryDirections[cpIndex[0]] +
            (dist_1 / total) * f.primaryDirections[cpIndex[1]] +
            (dist_2 / total) * f.primaryDirections[cpIndex[2]] +
            (dist_3 / total) * f.primaryDirections[cpIndex[3]];

        Vertex v = new Vertex {
            position = p,
            orientation = Quaternion.FromToRotation(gameObject.transform.forward, pDir)
        };

        return v;
    }

    int[] BilinearPoints(Vector3 p, List<Vector3> l) {
        int[] indexPoints = new int[4];

        float[] min = new float[4] { Mathf.Infinity, Mathf.Infinity, Mathf.Infinity, Mathf.Infinity };

        for (int i = 0; i < l.Count; i++) {
            float dist = Vector3.Distance(p, l[i]);

            // check for divide by 0
            if (float.IsNaN(dist)) continue;

            // 4 closest points in 4 quadrants around point
            if (dist < min[0] && l[i].x < p.x && l[i].y < p.y) {
                min[0] = dist;
                indexPoints[0] = i;
            } else if (dist < min[1] && l[i].x < p.x && l[i].y > p.y) {
                min[1] = dist;
                indexPoints[1] = i;
            } else if(dist < min[2] && l[i].x > p.x && l[i].y > p.y) {
                min[2] = dist;
                indexPoints[2] = i;
            } else if (dist < min[3] && l[i].x > p.x && l[i].y < p.y) {
                min[3] = dist;
                indexPoints[3] = i;
            }
        }

        return indexPoints;
    }
}