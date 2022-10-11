/**
* Copyright 2020 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

using IBM.Watson.TextToSpeech.V1;
using IBM.Watson.TextToSpeech.V1.Model;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IBM.Cloud.SDK;
using UnityEngine.Windows.Speech;
using System.Text;
using System;


namespace IBM.Watson.Examples
{
    public class ExampleTextToSpeechV1 : MonoBehaviour
    {
        #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
        [Space(10)]
        [Tooltip("The IAM apikey.")]
        [SerializeField]
        private string iamApikey;
        [Tooltip("The service URL (optional). This defaults to \"https://api.us-south.text-to-speech.watson.cloud.ibm.com\"")]
        [SerializeField]
        private string serviceUrl;
        private TextToSpeechService service;
        private string allisionVoice = "en-US_AllisonV3Voice";
        private string synthesizeText = "Hello, welcome to the Watson Unity SDK!";
        private string placeholderText = "Please type text here and press enter.";
        private string waitingText = "Watson Text to Speech service is synthesizing the audio!";
        private string synthesizeMimeType = "audio/wav";
        public InputField textInput;
        private bool _textEntered = false;
        private AudioClip _recording = null;
        private byte[] audioStream = null;
        #endregion
        float speed = 80f;

        TextMesh textObject;
       [SerializeField]
        private string[] m_Keywords;
        
        private KeywordRecognizer m_Recognizer;
        Text txt;
        private void Start()
        {
            LogSystem.InstallDefaultReactors();
            Runnable.Run(CreateService());
            textObject = GameObject.Find("Text").GetComponent<TextMesh>();


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
           
            Console.WriteLine(builder.ToString());
            
           
            Runnable.Run(ExampleSynthesize(args.text));
            switch (args.text)
            {
                case "step one":
                    Runnable.Run(ExampleSynthesize("Welcome to origami game. Start with your right hand and touch the green square."));

                    textObject.text = "Welcome to origami game. Start with your right hand and touch the green square.";



                    break;
                case "step two":
                    Runnable.Run(ExampleSynthesize("Start with your right hand and touch the green square."));

                   
                    textObject.text = "Start with your right hand and touch the green square.";

                    break;
                case "step three":
                    Runnable.Run(ExampleSynthesize("Start with your right hand and touch the green square."));


                    
                    textObject.text = "Start with your right hand and touch the green square.";





                    break;
                case "step four":
                    Runnable.Run(ExampleSynthesize(" Start with your right hand and touch the green square."));
                    textObject.text = "Start with your right hand and touch the green square.";
                    break;
                case "step five":
                    

                    Runnable.Run(ExampleSynthesize("Start with your right hand and touch the green square. Finished cube. You are done!"));
                    textObject.text = "Start with your right hand and touch the green square. Finished cube. You are done!";
                    break;
                
                   
              

                default:
                    Runnable.Run(ExampleSynthesize("I did not underestand. Please say the step number. For example say step one"));
                    Console.WriteLine("Invalid selection. Please select 1, 2, or 3.");
                    break;
            }

        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Runnable.Run(ExampleSynthesize(textInput.text));
            }
            transform.RotateAround(this.transform.position, new Vector3(0, 1, 0), speed * Time.deltaTime);
        }

        private IEnumerator CreateService()
        {
            if (string.IsNullOrEmpty(iamApikey))
            {
                throw new IBMException("Please add IAM ApiKey to the Iam Apikey field in the inspector.");
            }

            IamAuthenticator authenticator = new IamAuthenticator(apikey: iamApikey);

            while (!authenticator.CanAuthenticate())
            {
                yield return null;
            }

            service = new TextToSpeechService(authenticator);
            if (!string.IsNullOrEmpty(serviceUrl))
            {
                service.SetServiceUrl(serviceUrl);
            }
        }

        #region Synthesize Example
        public IEnumerator ExampleSynthesize(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = synthesizeText;
                Log.Debug("ExampleTextToSpeechV1", "Using default text, please enter your own text in dialog box!");

            }
            byte[] synthesizeResponse = null;
            AudioClip clip = null;
            service.Synthesize(
                callback: (DetailedResponse<byte[]> response, IBMError error) =>
                {
                    synthesizeResponse = response.Result;
                    Log.Debug("ExampleTextToSpeechV1", "Synthesize done!");
                    clip = WaveFile.ParseWAV("myClip", synthesizeResponse);
                    PlayClip(clip);
                },
                text: text,
                voice: allisionVoice,
                accept: synthesizeMimeType
            );

            while (synthesizeResponse == null)
                yield return null;

            yield return new WaitForSeconds(clip.length);
        }
        #endregion

        #region PlayClip
        private void PlayClip(AudioClip clip)
        {
            if (Application.isPlaying && clip != null)
            {
                GameObject audioObject = new GameObject("AudioObject");
                AudioSource source = audioObject.AddComponent<AudioSource>();
                source.spatialBlend = 0.0f;
                source.loop = false;
                source.clip = clip;
                source.Play();

                GameObject.Destroy(audioObject, clip.length);
            }
        }
        #endregion
    }
}
