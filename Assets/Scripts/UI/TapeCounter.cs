using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TapeCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tapeCount;

    private int _score = 0;

    // Start is called before the first frame update
    void Start()
    {
        _tapeCount.text = _score.ToString() + "/" + Gate.totalTape;
        Tape.OnAddScoreEvent += SetScoreDisplayer;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            _tapeCount.text = Gate.tapeCount + "/" + Gate.totalTape;
        }
    }

    private void SetScoreDisplayer(int score)
    {
        _tapeCount.text = score.ToString() + "/" + Gate.totalTape;
    }
}
