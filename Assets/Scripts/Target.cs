using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro.Fitts
{
    public class Target : MonoBehaviour
    {
        // audio clip 
        public AudioClip _aClip;

        // var: previous location
        Vector3 _prevPosition;

        // var: elapsed time
        float _elapsesTime;

        // var: total trials to run and current trial number
        public int trials = 50;
        int _count = 0;
        float _statTotalTime = 0.0f;
        float _statTotalDistance = 0.0f;

        // related UI
        //public Text _statUI;
        public TextMeshProUGUI _statUI;
        public GameObject _startUI;

        // Start is called before the first frame update
        void Start()
        {
            // initialize the prev position with the current
            _prevPosition = transform.localPosition;

            // zero elapsed time
            _elapsesTime = 0.0f;
        }

        // Update is called once per frame
        void Update()
        {
            // update elapsed time
            _elapsesTime += Time.deltaTime;

        }

        public void StartTest()
        {
            gameObject.SetActive(true);

            // initialize the prev position with the current
            _prevPosition = transform.localPosition;

            // zero elapsed time
            _elapsesTime = 0.0f;
            _count = 0;
            _statTotalDistance = 0f;
            _statTotalTime = 0f;

            Debug.Log("Test started.");
        }

        // call back for select event from the controller
        public void TargetClicked()
        {
            Debug.Log("Target selected.");

            // test: play some effect sound
            AudioSource.PlayClipAtPoint(_aClip, transform.position);

            // compute distance from the prev to current
            float dist = Vector3.Distance(_prevPosition, transform.localPosition);
            _statTotalDistance += dist;

            // update prev pos
            _prevPosition = transform.localPosition;

            // calc time taken and reset elapsed time
            float tasktime = _elapsesTime;
            _elapsesTime = 0.0f;
            _statTotalTime += tasktime;

            // store distance and task time to the file
            string outstr = dist.ToString() + "\t" + tasktime.ToString();
            DataLogger.Log(outstr);
            _statUI.text = _count.ToString() + " : " + outstr;

            // move the target to new location
            // i.e. range between (-8, 0, 20) to (8, 4, 20)
            float x = Random.Range(-10f, 10f);
            float y = Random.Range(0f, 5f);
            transform.localPosition = new Vector3(x, y, 20);

            // increment trial count
            _count++;
            if (_count == trials)
            {
                Debug.Log("Test completed.");

                _statUI.text = "avg distance: " + (_statTotalDistance / trials).ToString() + "\n" +
                               "avg time: " + (_statTotalTime / trials).ToString();

                gameObject.SetActive(false);
                _startUI.SetActive(true);

                // need to stop the test
                return;
            }
        }
    }
}