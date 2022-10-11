using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeRoof : MonoBehaviour
{
    private Renderer m_renderer;
    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_renderer.enabled = false;
        m_renderer.material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
