using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using fc;
//using vocab;

namespace FCFormVocab
{
    public partial class Form1 : Form
    {
        enum PromptState
        {
            init,
            prompting,
            answering
        }

        PromptState promptState;
        VocabEntry vocabEntry;
        int gobackIndex;

        List<VocabEntry> vocabEntryList = new List<VocabEntry>();

//        Verbiage verbiage;
//        List<Verbiage> verbiageList;
//        Conjugation conjugation;
        
        string basedir;
        public string BaseDir
        {
            get { return basedir; }
            set { basedir = value; }
        }

        public Form1()
        {
            InitializeComponent();
            // conjugation = Conjugation.Preterite;
            vocabEntryList = new List<VocabEntry>();
            vocabEntry = null;
            gobackIndex = 0;
        }
        private void formNextAction()
        {

            switch (promptState)
            {
                case PromptState.answering:
                    {
                        if (gobackIndex > 0)
                        {
                            gobackIndex = 0;
                            vocabEntry = vocabEntryList[vocabEntryList.Count - 2];
                        }
                        else
                        {
                            vocabEntry = vocab.getRandomVocabEntry();
                            vocabEntryList.Add(vocabEntry);
                        }
                        label1.Text = vocabEntry.portuguese; 
                        label2.Text = "";
                        label3.Text = "";

                        //if (verbiage.IsIrregular)
                        //{
                        //    label3.Text = "Irregular";
                        //}
                        promptState = PromptState.prompting;

                    }
                    break;
                case PromptState.prompting:
                    {
                        label1.Text = vocabEntry.genderToString() + " " + vocabEntry.portuguese; 
                        label3.Text = "";
                        label2.Text = "";
                        promptState = PromptState.answering;
                    }
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            formNextAction();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            promptState = PromptState.answering;
            string[] args = new string[1];

            args[0] = BaseDir;
            //Verbs.StaticConstructor(args);
            vocab.StaticConstructor(args);

            formNextAction();

        }

        private void Translate_Click(object sender, EventArgs e)
        {
            if (label3.Text == "")
                label3.Text = vocabEntry.english;
            else
                label3.Text = "";
        }

        private void Mistake_Click(object sender, EventArgs e)
        {
            VocabEntry ve; 
            if (gobackIndex > 0)
            {
                ve = vocabEntryList[vocabEntryList.Count - gobackIndex];
            }
            else 
            {
                ve = vocabEntry; 
            }
            vocab.recordMistake(ve, "NG" );

            label3.Text = "reminder noted for " + ve.portuguese;
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            vocab.removeEntries(vocabEntry);
            label3.Text = "entry removed";
        }

        private void Good_Click(object sender, EventArgs e)
        {
            VocabEntry ve; 
            if (gobackIndex > 0)
            {
                ve = vocabEntryList[vocabEntryList.Count - gobackIndex];
            }
            else 
            {
                ve = vocabEntry; 
            }
            vocab.recordMistake(ve, "OK" );

            label3.Text = "surprise noted for " + ve.portuguese;

        }

        private void Back_Click(object sender, EventArgs e)
        {
            gobackIndex++;
            if (gobackIndex < 1 || vocabEntryList.Count - gobackIndex < 0)
            {
                label3.Text = "zeroed";
                gobackIndex = 0;
            }
            else
            {
                label3.Text = vocabEntryList[vocabEntryList.Count - gobackIndex].portuguese;
            }
        }

        private void Forward_Click(object sender, EventArgs e)
        {
            gobackIndex--;
            if (gobackIndex < 1 || vocabEntryList.Count - gobackIndex < 0)
            {
                label3.Text = "zeroed";
                gobackIndex = 0;
            }
            else
            {
                label3.Text = vocabEntryList[vocabEntryList.Count - gobackIndex].portuguese;
            }
        }

    }
}
