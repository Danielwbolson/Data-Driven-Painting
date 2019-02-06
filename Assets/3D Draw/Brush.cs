using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour {

    GameObject painting;
    private Stroke _stroke;
    private Stroke streamline;
    private Vector3 _cursorPosition;
    private Vector3 _recentCursorPosition;
    private Vector3 _lastVertexPosition;

    private float _drawBuffer = 0.005f;

    private bool _currDrawing = false;

    public FakeData fd;
    private VRTK.VRTK_InteractUse vrtk_use;

    void Start() {
        painting = new GameObject("Painting");

        fd = GameObject.Find("DummyDataHandler").GetComponent<FakeData>();
        vrtk_use = GetComponent<VRTK.VRTK_InteractUse>();
    }

    // Update is called once per frame
    void Update() {

        // Update our cursor position every frame
        _cursorPosition = transform.position;

        // If we are starting our drawing
        if (!_currDrawing && (Input.GetMouseButtonDown(0) || vrtk_use.IsUseButtonPressed())) {
            // Create a gameobject that will be our drawing (Painting?)
            GameObject stroke = new GameObject("Stroke");
            stroke.transform.parent = painting.transform;

            // Set our variable for currently drawing
            _currDrawing = true;

            // Cache this position for deciding when to add the next vertex
            _lastVertexPosition = _cursorPosition;

            // Start our stroke
            _stroke = stroke.AddComponent<Stroke>();
            _stroke.buffer = _drawBuffer;

            Vertex v = new Vertex {
                position = _cursorPosition,
                direction = Vector3.Normalize(_cursorPosition - _recentCursorPosition)
            };

            _stroke.AddVertex(v);
        }

        // If we are still drawing and have moved far enough away from our last position, continue drawing
        if (_currDrawing && Vector3.Distance(_cursorPosition, _lastVertexPosition) > _drawBuffer) {
            // Extend our drawing
            Vertex v = new Vertex {
                position = _cursorPosition,
                direction = Vector3.Normalize(_cursorPosition - _recentCursorPosition)
            };

            _stroke.AddVertex(v);
        }

        // IF the user has stopped drawing, reset our variable
        if (Input.GetMouseButtonUp(0) && !vrtk_use.IsUseButtonPressed() && _currDrawing) {
            _currDrawing = false;
            StartCoroutine(_stroke.MorphToData(fd));
        }
        _recentCursorPosition = _cursorPosition;
    }
}
