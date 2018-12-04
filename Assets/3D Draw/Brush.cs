using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour {

    GameObject painting;
    private Stroke _stroke;
    private Vertex _cursorPosition;
    private Vertex _recentCursorPosition;

    private float _drawBuffer = 0.01f;

    private bool _currDrawing = false;

    public FakeData fd;

    void Start() {
        painting = new GameObject("Painting");

        fd = GameObject.Find("DummyDataHandler").GetComponent<FakeData>();
    }

    // Update is called once per frame
    void Update() {

        // If we are in drawing mode, update our cursor every frame
        if (_currDrawing) {
            _cursorPosition.position = transform.position;
            _cursorPosition.orientation = transform.rotation;
        }

        // If we are starting our drawing
        if (Input.GetMouseButtonDown(0)) {
            // Create a gameobject that will be our drawing (Painting?)
            GameObject stroke = new GameObject("Stroke");
            stroke.transform.parent = painting.transform;

            // Set our variable for currently drawing
            _currDrawing = true;

            // Set our cursor position to our current position
            _cursorPosition = new Vertex {
                position = transform.position,
                orientation = transform.rotation
            };

            // Cache our cursor position
            _recentCursorPosition = _cursorPosition;

            // Start our stroke
            _stroke = stroke.AddComponent<Stroke>();
            _stroke.buffer = _drawBuffer;
            _stroke.AddVertex(_cursorPosition);
        }

        // If we are still drawing and have moved far enough away from our last position, continue drawing
        if (_currDrawing && Vector3.Distance(_cursorPosition.position, _recentCursorPosition.position) > _drawBuffer) {
            // Reset our cached position to the current one
            _recentCursorPosition = _cursorPosition;

            // Extend our drawing
            _stroke.AddVertex(_cursorPosition);
        }

        // IF the user has stopped drawing, reset our variable
        if (Input.GetMouseButtonUp(0)) {
            _currDrawing = false;
            _stroke.MorphToData(fd);
        }
    }
}
