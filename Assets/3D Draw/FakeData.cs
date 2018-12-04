﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeData : MonoBehaviour {

    public List<Vector3> positions;
    public List<Vector3> primaryDirections;
    public Material mat;
    private const int width = 20;

    float maxX = -Mathf.Infinity;
    float maxY = -Mathf.Infinity;
    float maxZ = -Mathf.Infinity;
    float minX = Mathf.Infinity;
    float minY = Mathf.Infinity;
    float minZ = Mathf.Infinity;

    // Use this for initialization
    void Start() {
        positions = new List<Vector3>(new Vector3[width * width * width]);

        // i : vert, j : horizontal, k : forward
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < width; j++) {
                for (int k = 0; k < width; k++) {
                    int ind = index(i, j, k);
                    float x = i * 0.1f; float y = j * 0.1f; float z = k * 0.1f;
                    positions[ind] = new Vector3(x, y, z);

                    if (x > maxX) maxX = x;
                    if (x < minX) minX = x;
                    if (y > maxY) maxY = y;
                    if (y < minY) minY = y;
                    if (z > maxZ) maxZ = z;
                    if (z < minZ) minZ = z;
                }
            }
        }

        primaryDirections = new List<Vector3>(new Vector3[width * width * width]);
        // i : vert, j : horizontal, k : forward
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < width; j++) {
                for (int k = 0; k < width; k++) {
                    primaryDirections[index(i, j, k)] = 
                        Vector3.Normalize(new Vector3(1, j / (float)width, 1));
                }
            }
        }

        // i : vert, j : horizontal, k : forward
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < width; j++) {
                for (int k = 0; k < width; k++) {
                    GameObject g = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    g.transform.position = positions[index(i, j, k)];
                    g.transform.up = primaryDirections[index(i, j, k)];
                    g.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);

                    g.GetComponent<MeshRenderer>().material = mat;
                }
            }
        }
    }

    int index(int i, int j, int k) {
        return (i * width * width) + (j * width) + k;
    }

    public bool ValidData(Vertex v) {
        Vector3 p = v.position;

        if (p.x < minX || p.x > maxX || p.y < minY || p.y > maxY || p.z < minZ || p.z > maxZ) {
            return false;
        }

        return true;
    }
}