using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MovingBG : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private List<RectTransform> _bgTransforms = new List<RectTransform>();
    private Vector3 _updateTranslation = Vector3.zero;
    private Vector3 _tpTranslation = Vector3.zero;
    private float _imageLength = 1920;
    private float _lengthToTP = float.MaxValue;

    void Awake()
    {
        _imageLength = _bgTransforms[0].GetComponent<RectTransform>().rect.width;
        _updateTranslation = new Vector3(-_speed, 0, 0);
        _tpTranslation = new Vector3(_bgTransforms.Count * _imageLength, 0, 0);
        _lengthToTP = -_imageLength * 1.5f;
    }

    private void FixedUpdate()
    {
        Vector3 adjustedTranslation = _updateTranslation * Time.deltaTime;
        foreach(Transform transform in _bgTransforms)
        {
            transform.Translate(adjustedTranslation);
            if (transform.localPosition.x <= _lengthToTP)
            {
                TPImage(transform);
            }
        }
    }

    private void TPImage(Transform transform)
    {
        transform.localPosition += _tpTranslation;
    }
}
