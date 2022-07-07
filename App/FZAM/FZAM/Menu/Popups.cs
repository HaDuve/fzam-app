using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FZAM.Menu
{
    public enum PopupType
    {
        Invalid,
        InfoPositivesCoping,
        InfoDysfunktionalesLampenfieber,
        InfoSelfAssessment,
        InfoJudgmentScale,
        FirstQuestionnairePopUp,
        FirstResultsPopUp,
        FirstResultsSelectionPopUp,
        TooFewPerformancesSelected,
        TooManyPerformancesSelected
    }

    public class Popups
    {
        private readonly Page Menu;

        public Popups(Page menu)
        {
            this.Menu = menu;
        }

        public void Display(PopupType type)
        {
            switch (type)
            {
                /*case PopupType.InfoPositivesCoping:
                    // using the functions with the same - int at the end of name -  as in this category /enum PopupType
                    var averagesFunc = this.Menu.GetAveragesForPopup1();
                    if (averagesFunc != null)
                        //this.InfoPositivesCoping(averagesFunc);
                        this.InfoPositivesCoping();
                    else
                        this.InfoPositivesCoping();
                    break;
                case PopupType.InfoDysfunktionalesLampenfieber:
                    var averagesDysf = this.Menu.GetAveragesForPopup2();
                    if (averagesDysf != null)
                        // this.InfoDysfunktionalesLampenfieber(averagesDysf);
                        this.InfoDysfunktionalesLampenfieber();
                    else
                        this.InfoDysfunktionalesLampenfieber();
                    break;
                case PopupType.InfoSelfAssessment:
                    var averagesSelf = this.Menu.GetAveragesForPopup3();
                    if (averagesSelf != null)
                        //this.InfoSelfAssessment(averagesSelf);
                        this.InfoSelfAssessment();
                    else
                        this.InfoSelfAssessment();
                    break;
                case PopupType.InfoJudgmentScale:
                    var averagesJudge = this.Menu.GetAveragesForPopup4();
                    break;*/
                case PopupType.Invalid:
                    break;
                case PopupType.TooFewPerformancesSelected:
                    this.TooFewPerfomancesSelected();
                    break;
                case PopupType.TooManyPerformancesSelected:
                    this.TooManyPerfomancesSelected();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void TooFewPerfomancesSelected()
        {
            this.Menu.DisplayAlert("Zu wenige Auftritte ausgewählt.",
                "Für die Darstellung der Einzelauftritte wählen Sie bitte " +
                "mindestens zwei Auftritte aus.", "OK");
        }

        private void TooManyPerfomancesSelected()
        {
            this.Menu.DisplayAlert("Zu viele Auftritte ausgewählt.",
                "Für die Darstellung der Einzelauftritte wählen Sie bitte " +
                "weniger als elf Auftritte aus.", "OK");
        }


        // TODO: load these popups via json
        protected void InfoPositivesCoping()
        {
            //  . . . Bei 500 befragten Musiker*innen lag der Durchschnittswert bei 4.
            this.Menu.DisplayAlert("Positives Coping",
                "Wie gut kann ich mit den Erscheinungen meines Lampenfiebers umgehen? \n Lampenfieber gehört zum Auftritt dazu und je gelassener und positiver wir damit umgehen, " +
                "um so besser gelingt der Auftritt. Je höher der Wert auf dieser Skala ist, desto positiver ist das Coping, also der Umgang mit Lampenfieber. \n Dieser Wert sollte " +
                "vor dem Auftritt, während des Auftritts und nach dem Auftritt möglichst hoch ausfallen.",
                "Ok");
        }

        protected void InfoDysfunktionalesLampenfieber()
        {
            // . . . Bei 500 befragten Musiker*innen lag der Durchschnittswert bei 2.
            this.Menu.DisplayAlert("Dysfunktionales Lampenfieber",
                "Wie stark sind die Erscheinungen meines Lampenfiebers und wie sehr lasse ich mich davon beeinträchtigen? Lampenfieber wirkt sich dann negativ auf den Auftritt aus, " +
                "wenn wir uns davon bestimmen und verunsichern lassen. Je höher der Wert auf dieser Skala ist, desto ungünstiger ist die Bewältigungsstrategie. Dieser Wert sollte vor " +
                "dem Auftritt während des Auftritts und nach dem Auftritt möglichst niedrig ausfallen.",
                "Ok");
        }

        protected void InfoSelfAssessment()
        {
            //. . .Bei 500 befragten Musiker*innen lag der Durchschnittswert bei 4.
            this.Menu.DisplayAlert("Selbstwirksamkeit",
                "Wie sehr bin ich davon überzeugt, die Herausforderungen des Auftritts zu meistern? Eine starke Überzeugung, dass ich meine Ziele beim Auftritt erreichen kann, " +
                "erhöht auch die Wahrscheinlichkeit, dass der Auftritt gelingt. Je höher der Wert, desto größer ist die Selbstwirksamkeit. Dieser Wert sollte vor dem Auftritt, " +
                "während des Auftritts und nach dem Auftritt möglichst hoch ausfallen.",
                "Ok");
        }

    }
}