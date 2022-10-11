using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

using UnityEngine.Windows.Speech;
using IBM.Watson.Examples;

public class KeywordScript : MonoBehaviour
{
    [SerializeField]
    private string[] m_Keywords;

    private KeywordRecognizer m_Recognizer;

    void Start()
    {
        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());
        Console.WriteLine(builder.ToString());
        //ExampleTextToSpeechV1 sn = gameObject.GetComponent<ExampleTextToSpeechV1>();
        //sn.ExampleSynthesize(builder.ToString());

    }
}
