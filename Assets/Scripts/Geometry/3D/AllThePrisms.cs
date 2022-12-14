using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllThePrisms : AbstractMeshGenerator {
    [SerializeField, Range(3, 50)] private int numSides = 3;
    [SerializeField] private float frontRadius;
    [SerializeField] private float backRadius;
    [SerializeField] private float length;

    private Vector3[] vs;

    [SerializeField] private Gradient gradient;
    protected override void SetMeshNum() {
        numVertices = 6 * numSides; // numSides vertices on each end, 4 on each length-side
        numTriangles = 12 * (numSides - 1); // There are (numSides - 2) on each end and 6 on each length-side: 6 * numsides
    }
    protected override void SetVertecies() {
        // Cordinates of a regular polygon
        vs = new Vector3[numSides * 2];

        // Set the vs
        for (int i = 0; i < numSides; i++) {
            float angle = 2 * Mathf.PI * i / numSides;
            // One end
            vs[i] = new Vector3(frontRadius * Mathf.Cos(angle), frontRadius * Mathf.Sin(angle), 0);
            // Other end
            vs[i + numSides] = new Vector3(backRadius * Mathf.Cos(angle), backRadius * Mathf.Sin(angle), length);
        }

        // Set Vertices - First End
        for (int i = 0; i < numSides; i++) {
            vertices.Add(vs[i]);
        }

        // Set Vertices - Middle Verts
        for (int i = 0; i < numSides; i++) {
            vertices.Add(vs[i]);
            int secondIndex = i == 0 ? 2 * numSides - 1 : numSides + i - 1;
            vertices.Add(vs[secondIndex]);
            int thirdIndex = i == 0 ? numSides - 1 : i - 1;
            vertices.Add(vs[thirdIndex]);
            vertices.Add(vs[numSides + i]);
        }

        // Set Vertices - Other End
        for (int i = 0; i < numSides; i++) {
            vertices.Add(vs[i + numSides]);
        }
    }
    protected override void SetTriangles() {
        // First End
        for (int i = 1; i < numSides - 1; i++) {
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i);
        }

        // Middle
        for (int i = 1; i <= numSides; i++) {

            //There are numSides triangles in the first end, so start at numSides. On each loop, need to increase. 4*(i-1) does this correctly
            int val = numSides + 4 * (i - 1);

            triangles.Add(val);
            triangles.Add(val + 1);
            triangles.Add(val + 2);

            triangles.Add(val);
            triangles.Add(val + 3);
            triangles.Add(val + 1);
        }

        // Other End - Opposite way round so face points outwards
        for (int i = 1; i < numSides - 1; i++) {
            // There are numSides trianglesi n the first end, 4 * numSides triangles in the middle, so this starts on 5 * numSides
            triangles.Add(5 * numSides);
            triangles.Add(5 * numSides + i);
            triangles.Add(5 * numSides + i + 1);
        }
    }
    protected override void SetVertexColours() {
        // Other End - Opposite way round so face points outwards
        for (int i = 0; i < numVertices; i++) {
            // Use the values in the gradient to colour
            vertexColours.Add(gradient.Evaluate((float)i / numVertices));
        }
    }
    protected override void SetUVs() { 
        // Poligon End
        for (int i = 0; i < numSides; i++) {
            uvs.Add(vs[i]);
        }

        // Middle
        for (int i = 0; i < numSides; i++) {
            // The sides are all Rectangles 
            uvs.Add(new Vector2(frontRadius, 0));
            uvs.Add(new Vector2(0, length));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(backRadius, length));

        }

        // Other Polygon End
        for (int i = 0; i < numSides; i++) {
            uvs.Add(vs[i + numSides]);
        }
    }

    protected override void SetNormals() { }
    protected override void SetTangents() { }
}
