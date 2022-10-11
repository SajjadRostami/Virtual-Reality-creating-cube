using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerLeftTailSubSciript : MonoBehaviour
{
    private Renderer m_renderer;

    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_renderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}