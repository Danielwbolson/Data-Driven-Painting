using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeData : MonoBehaviour {

    public int dataSwitch;

    public List<Vector3> positions;
    public List<Vector3> primaryDirections;
    private List<Matrix4x4> matrices;
    public Material mat;
    public Mesh mesh;
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
                        float r = width / 40.0f * Mathf.Sqrt(u);

                        int ind = index(i, j, k);
                        float x = r * Mathf.Sin(theta) * Mathf.Cos(phi);
                        float y = r * Mathf.Sin(theta) * Mathf.Sin(phi);
                        float z = r * Mathf.Cos(theta);
                        positions[ind] = new Vector3(x, y, z);
                        positions[ind] += transform.position;

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
                                    positions[index(i, j, k)] - transform.position,
                                    gameObject.transform.up) +
                                    new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f)));
                    }
                }
            }
        } else if (dataSwitch == 1) {
            // i : vert, j : horizontal, k : forward
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < width; j++) {
                    for (int k = 0; k < width; k++) {
                        int ind = index(i, j, k);
                        float x = i * 0.05f + Random.Range(0f, 0.1f);
                        float y = j * 0.05f + Random.Range(0f, 0.1f);
                        float z = k * 0.05f + Random.Range(0f, 0.1f);
                        positions[ind] = new Vector3(x, y, z);
                        positions[ind] += transform.position;

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
            float t = 0.1f;
            // i : vert, j : horizontal, k : forward
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < width; j++) {
                    for (int k = 0; k < width; k++) {
                        primaryDirections[index(i, j, k)] =
                            Vector3.Normalize(new Vector3(j, -i, k));
                    }
                }
            }
        } else if (dataSwitch == 2) {
            // i : vert, j : horizontal, k : forward
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < width; j++) {
                    for (int k = 0; k < width; k++) {
                        int ind = index(i, j, k);
                        float x = i * 0.05f + Random.Range(0f, 0.1f);
                        float y = j * 0.05f + Random.Range(0f, 0.1f);
                        float z = k * 0.05f + Random.Range(0f, 0.1f);
                        positions[ind] = new Vector3(x, y, z);
                        positions[ind] += transform.position;

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
                            Vector3.Normalize(new Vector3(i * Mathf.Abs(j - width / 2.0f), j * Mathf.Abs(k - width / 2.0f), k * Mathf.Abs(i - width / 2.0f)));
                    }
                }
            }
        }

        matrices = new List<Matrix4x4>(new Matrix4x4[width * width * width]);

        // i : vert, j : horizontal, k : forward
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < width; j++) {
                for (int k = 0; k < width; k++) {
                    matrices[index(i, j, k)] = Matrix4x4.Translate(positions[index(i, j, k)]);
                    matrices[index(i, j, k)] *= Matrix4x4.Rotate(Quaternion.LookRotation(primaryDirections[index(i, j, k)], Vector3.up));
                    matrices[index(i, j, k)] *= Matrix4x4.Scale(new Vector3(0.005f, 0.005f, 0.02f));
                }
            }
        }
    }

    void Update() {
        for (int i = 0; i < matrices.Count; i++) {
            Graphics.DrawMesh(mesh, matrices[i], mat, 0);
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