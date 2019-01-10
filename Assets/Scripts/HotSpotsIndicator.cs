using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotSpotsIndicator : Graphic
{
    [SerializeField] private List<Vector2> _hotSpots = new List<Vector2>();

    private RectTransform _transform
    {
        get { return (RectTransform) transform; }
    }

    public float Z;
    public float Size;

    public void SetHotSpots(List<Vector2> hotspots)
    {
        _hotSpots = hotspots;
        SetVerticesDirty();
    }

    
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        var offset = Size / 2;
        
        vh.Clear();
        
        var topLeft = new Vector2(_transform.rect.x, _transform.rect.yMax);
        var bottomRight = new Vector2(_transform.rect.xMax, _transform.rect.y);

        var vertices = new List<UIVertex>(_hotSpots.Count * 3);
        for (int i = 0; i < _hotSpots.Count; i++)
        {
            var hotSpot = _hotSpots[i];

            var normalizedX = Mathf.LerpUnclamped(topLeft.x, bottomRight.x, hotSpot.x);
            var normalizedY = Mathf.LerpUnclamped(topLeft.y, bottomRight.y, hotSpot.y);

            
            var v1 = new UIVertex()
            {
                position = new Vector3(normalizedX - offset, normalizedY + offset, Z),
                color = this.color
            };
            var v2 = new UIVertex()
            {
                position = new Vector3(normalizedX + offset, normalizedY + offset, Z),
                color = this.color
            };
            var v3 = new UIVertex()
            {
                position = new Vector3(normalizedX, normalizedY - offset, Z),
                color = this.color,
                
            };
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
        }
        vh.AddUIVertexTriangleStream(vertices);
    }
    
}
