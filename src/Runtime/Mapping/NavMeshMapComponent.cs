using UnityEngine;

namespace MyRpg.Mapping
{
    public class NavMeshMapComponent : MonoBehaviour
    {
        /*
        [SerializeField] private Vector2 size;
        [SerializeField] private Vector2 origin;
        [SerializeField] private float resolution;

        private void Awake()
        {
            var walkableArea = GetWalkableArea();


            var png = ImageConversion.EncodeToPNG(texture);
            File.WriteAllBytes("C:\\Users\\olive\\map.png", png);
        }

        private List<List<bool>> GetWalkableArea()
        {
            var map = new List<List<bool>>();
            
            for (var x = origin.x - size.x / 2; x < origin.x + size.x / 2; x += resolution)
            {
                var row = new List<bool>();
                for (var y = origin.y - size.y / 2; y < origin.y + size.y / 2; y += resolution)
                {
                    var p = new Vector3(x, y);
                    var b = NavMesh.SamplePosition(p, out var hit, resolution, NavMesh.AllAreas);
                    row.Add(b);
                }
                map.Add(row);
            }

            return map;
        }

        private Texture2D GetMapEdges(Color wallColor, Color backgroundColor)
        {
            var texture = new Texture2D(_walkableArea.Count, _walkableArea[0].Count);
            for (int i = 1; i < _walkableArea.Count - 1; i++)
            {
                var row = _walkableArea[i];
                for (var j = 1; j < row.Count - 1; j++)
                {
                    var walkableNeighbor =
                        _walkableArea[i - 1][j] ||
                        _walkableArea[i][j - 1] ||
                        _walkableArea[i + 1][j] ||
                        _walkableArea[i][j + 1];

                    var edge = row[j] == false && walkableNeighbor;
                    var color = edge ? Color.white : Color.black;
                    texture.SetPixel(i, j, color);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(origin, size);
        }
        
        // Get Map with origin and size.
        // As texture with transparent background and black edges?
        // On top of texture passed in?
        */
    }
}
