using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Randomizes transforms of children of objects referred through the editor
/// used to randomize the trees in the main game scene
/// </summary>
public class RandomizeTransform : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> _randomizeChildrenOf = new List<GameObject>();

    [SerializeField]
    private Vector3 _positionRange;

    [SerializeField]
    private Vector3 _scaleRange;

    [SerializeField]
    private Vector3 _rotationRange;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _randomizeChildrenOf.Count; ++i)
        {
            for (int j = 0; j < _randomizeChildrenOf[i].transform.childCount; ++j)
            {
                Transform t = _randomizeChildrenOf[i].transform.GetChild(j);
                t.position = Vector3.Lerp(t.position - _positionRange, t.position + _positionRange, Random.Range(0.0f, 1.0f));
                t.localScale = Vector3.Lerp(t.localScale - _scaleRange, t.localScale + _scaleRange, Random.Range(0.0f, 1.0f));
                t.rotation = Quaternion.Euler(Vector3.Lerp(t.rotation.eulerAngles - _rotationRange, t.eulerAngles + _rotationRange, Random.Range(0.0f, 1.0f)));
            }

        }

        Destroy(this);
    }
}
