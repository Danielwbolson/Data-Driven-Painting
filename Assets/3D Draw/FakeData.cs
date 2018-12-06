using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeData : MonoBehaviour {

    public int dataSwitch;

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

        if (dataSwitch == 0) {
            // i : vert, j : horizontal, k : forward
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < width; j++) {
                    for (int k = 0; k < width; k++) {

                        float phi = Random.Range(0, 2 * 3.14159f);
                        float cosTheta = Random.Range(-1f, 1f);
                        float u = Random.Range(0f, 1f);

                        float theta = Mathf.Acos(cosTheta);
                        float r = width / 10.0f * Mathf.Sqrt(u);

                        int ind = index(i, j, k);
                        float x = r * Mathf.Sin(theta) * Mathf.Cos(phi);
                        float y = r * Mathf.Sin(theta) * Mathf.Sin(phi);
                        float z = r * Mathf.Cos(theta);
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
                            Vector3.Normalize(
                                Vector3.Cross(
                                    positions[index(i, j, k)],
                                    gameObject.transform.up) +
                                    new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)));
                    }
                }
            }
        } else if (dataSwitch == 1) {
            // i : vert, j : horizontal, k : forward
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < width; j++) {
                    for (int k = 0; k < width; k++) {
                        int ind = index(i, j, k);
                        float x = i * 0.1f + Random.Range(0f, 0.1f);
                        float y = j * 0.1f + Random.Range(0f, 0.1f);
                        float z = k * 0.1f + Random.Range(0f, 0.1f);
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
                            Vector3.Normalize(new Vector3(i * i, j * j, k * k));
                    }
                }
            }
        } else if (dataSwitch == 2) {
            // i : vert, j : horizontal, k : forward
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < width; j++) {
                    for (int k = 0; k < width; k++) {
                        int ind = index(i, j, k);
                        float x = i * 0.1f + Random.Range(0f, 0.1f);
                        float y = j * 0.1f + Random.Range(0f, 0.1f);
                        float z = k * 0.1f + Random.Range(0f, 0.1f);
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
                            Vector3.Normalize(new Vector3(i * Mathf.Abs(j - width / 2.0f), j * Mathf.Abs(i - width / 2.0f), 1));
                    }
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
                    if (Random.Range(0, 2) == 0)
                        g.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0.2f);
                    else
                        g.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0.2f);
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