using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using fc;

namespace FCForm
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

        Verbiage verbiage;
        List<Verbiage> verbiageList;
        Conjugation conjugation;
        string basedir;
        int gobackIndex; 

        public string BaseDir
        {
            get { return basedir; }
            set { basedir = value; }
        }

        private void IncrementConjugation()
        {
            Array a = Enum.GetValues(typeof(Conjugation));
            if ((int)++conjugation >= a.Length)
                conjugation = 0;
        }

        public Form1()
        {

            InitializeComponent();

            conjugation = Conjugation.Present;
            verbiageList = new List<Verbiage>();
            verbiage = null;
        }

        private void formNextAction()
        {

            switch (promptState)
            {
                case PromptState.answering:
                    {
                        verbiage = Verbs.GetAConjugation(conjugation);
                        verbiageList.Add(verbiage);
                        label1.Text = verbiage.Infinitive;
                        label2.Text = verbiage.Pronoun.ToString();
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
                        label1.Text = "";
                        label3.Text = "";
                        label2.Text = verbiage.Pronoun.ToString() + " " + verbiage.ConjugatedVerb;
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
            Verbs.StaticConstructor(args);

            formNextAction();
        }

        private void Good_Click(object sender, EventArgs e)
        {
            Verbiage v = verbiage;

#if (compileunwanted)
             ve;
            if (gobackIndex > 0)
            {
                ve = vocabEntryList[vocabEntryList.Count - gobackIndex];
            }
            else
            {
                ve = vocabEntry;
            }
            vocab.recordMistake(ve, "OK");

            label3.Text = "surprise noted for " + ve.portuguese;

#endif
        }

        private void translate_Click(object sender, EventArgs e)
        {
            IncrementConjugation();
            label3.Text = conjugation.ToString();
        }

        private void mistake_Click(object sender, EventArgs e)
        {
            Verbiage ve;

            if (gobackIndex > 0)
            {
                ve = verbiageList[verbiageList.Count - gobackIndex];
            }
            else
            {
                ve = verbiage;
            }
            Verbs.recordMistake(ve, "NG");

            label3.Text = "reminder noted for " + ve.Infinitive; 
        }

        private void remove_Click(object sender, EventArgs e)
        {
            Verbs.removeInfinitive(verbiage.Infinitive);
        }

        private void back_Click(object sender, EventArgs e)
        {
            gobackIndex++;
            if (gobackIndex < 1 || verbiageList.Count - gobackIndex < 0)
            {
                label3.Text = "zeroed";
                gobackIndex = 0;
            }
            else
            {
                label3.Text = verbiageList[verbiageList.Count - gobackIndex].Infinitive;
            }
        }

        private void forward_Click_1(object sender, EventArgs e)
        {
            gobackIndex--;
            if (gobackIndex < 1 || verbiageList.Count - gobackIndex < 0)
            {
                label3.Text = "zeroed";
                gobackIndex = 0;
            }
            else
            {
                label3.Text = verbiageList[verbiageList.Count - gobackIndex].Infinitive;
            }
        }

        private void good_Click_1(object sender, EventArgs e)
        {
            Verbiage ve;

            if (gobackIndex > 0)
            {
                ve = verbiageList[verbiageList.Count - gobackIndex];
            }
            else
            {
                ve = verbiage;
            }
            Verbs.recordMistake(ve, "OK");

            label3.Text = "reminder noted for " + ve.Infinitive; 

        }

    }
}
