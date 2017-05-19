using CreativeSpore.SuperTilemapEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode] //fix ShouldRunBehaviour warning when using OnTilePrefabCreation
public class TileData : MonoBehaviour
{
    public Tilemap Map;
    public Tile Tile;
    public Vector2 Position;

    void OnTilePrefabCreation(TilemapChunk.OnTilePrefabCreationData data)
    {
        Map = data.ParentTilemap;
        Tile = data.ParentTilemap.GetTile(data.GridX, data.GridY);
        Position.Set(data.GridX, data.GridY);
        
        if (Tile != null)
        {
            float pixelsPerUnit = data.ParentTilemap.Tileset.TilePxSize.x / data.ParentTilemap.CellSize.x;
            Vector2 atlasSize = new Vector2(data.ParentTilemap.Tileset.AtlasTexture.width, data.ParentTilemap.Tileset.AtlasTexture.height);
            Rect spriteUV = new Rect(Vector2.Scale(Tile.uv.position, atlasSize), Vector2.Scale(Tile.uv.size, atlasSize));
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Sprite.Create(data.ParentTilemap.Tileset.AtlasTexture, spriteUV, new Vector2(.5f, .5f), pixelsPerUnit);
            spriteRenderer.sortingLayerID = data.ParentTilemap.SortingLayerID;
            spriteRenderer.sortingOrder = data.ParentTilemap.OrderInLayer;
        }
    }
}
