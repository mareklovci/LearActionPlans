using System;
using System.Linq;
using System.Windows.Forms;
using LearActionPlans.DataMappers;
using LearActionPlans.Models;

namespace LearActionPlans.Views
{
    public partial class FormZadaniBoduAP
    {
        private void UlozitBodAP()
        {
            var ulozit = true;

            //nejdřív proběhne test na vyplnění položek
            //this.RichTextBoxPopisProblemu.BackColor = SystemColors.Window;

            if (string.IsNullOrWhiteSpace(this.RichTextBoxPopisProblemu.Text))
            {
                _ = MessageBox.Show("You must fill in the Problem description field.", "Notice", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ulozit = false;
                //this.RichTextBoxPopisProblemu.BackColor = Color.Yellow;
            }

            //if (ulozit == false)
            //{
            //    MessageBox.Show("The yellow cells must be filled.", "Notice", MessageBoxButtons.OK,
            //        MessageBoxIcon.Information);
            //}

            if (Convert.ToInt32(this.ComboBoxOdpovednaOsoba1.SelectedValue) == 0)
            {
                ulozit = false;
                _ = MessageBox.Show("You must select a Responsible employee #1.", "Notice", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            if (Convert.ToInt32(this.ComboBoxOddeleni.SelectedValue) == 0)
            {
                ulozit = false;
                _ = MessageBox.Show("You must select a Department.", "Notice", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            if (!ulozit)
            {
                return;
            }

            var reopen = this.znovuOtevritAP == 0;

            //proměnnou  ulozitBodAP asi potřebovat nebudu
            BodAP ulozitBodAP;

            if (this.novyBodAP == true)
            {
                var odpovednaOsoba2Id = Convert.ToInt32(this.ComboBoxOdpovednaOsoba2.SelectedValue) == 0
                    ? (int?)null
                    : Convert.ToInt32(this.ComboBoxOdpovednaOsoba2.SelectedValue);

                //založí nový bod
                FormPrehledBoduAP.bodyAP.Add(new BodAP(this.akcniPlany_.Id,
                    FormPrehledBoduAP.bodyAP.Count + 1,
                    DateTime.Now, this.TextBoxOdkazNaNormu.Text, this.TextBoxHodnoceniNeshody.Text,
                    this.RichTextBoxPopisProblemu.Text, this.RichTextBoxSkutecnaPricinaWM.Text,
                    this.RichTextBoxNapravnaOpatreniWM.Text, this.RichTextBoxSkutecnaPricinaWS.Text,
                    this.RichTextBoxNapravnaOpatreniWS.Text,
                    Convert.ToInt32(this.ComboBoxOdpovednaOsoba1.SelectedValue),
                    odpovednaOsoba2Id, this.kontrolaEfektivnostiDatum,
                    Convert.ToInt32(this.ComboBoxOddeleni.SelectedValue), this.priloha, this.datumUkonceni, this.poznamkaDatumUkonceni,
                    reopen,
                    false,
                    1));

                ulozitBodAP = FormPrehledBoduAP.bodyAP.Last();
            }
            else
            {
                //aktualizace stávajícího bodu
                ulozitBodAP = FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody];

                ulozitBodAP.OdkazNaNormu = this.TextBoxOdkazNaNormu.Text;
                ulozitBodAP.HodnoceniNeshody = this.TextBoxHodnoceniNeshody.Text;
                ulozitBodAP.PopisProblemu = this.RichTextBoxPopisProblemu.Text;
                ulozitBodAP.DatumUkonceni = this.datumUkonceni;
                ulozitBodAP.UkonceniPoznamka = this.poznamkaDatumUkonceni;
                //ulozitBodAP.SkutecnaPricinaWM = this.RichTextBoxSkutecnaPricinaWM.Text;
                //ulozitBodAP.NapravnaOpatreniWM = this.RichTextBoxNapravnaOpatreniWM.Text;
                //ulozitBodAP.SkutecnaPricinaWS = this.RichTextBoxSkutecnaPricinaWS.Text;
                //ulozitBodAP.NapravnaOpatreniWS = this.RichTextBoxNapravnaOpatreniWS.Text;
                ulozitBodAP.OdpovednaOsoba1Id = Convert.ToInt32(this.ComboBoxOdpovednaOsoba1.SelectedValue);
                ulozitBodAP.OdpovednaOsoba2Id = this.ComboBoxOdpovednaOsoba2.SelectedIndex == 0
                    ? 0
                    : Convert.ToInt32(this.ComboBoxOdpovednaOsoba2.SelectedValue);
                ulozitBodAP.OddeleniId = this.ComboBoxOddeleni.SelectedIndex == 0
                    ? 0
                    : Convert.ToInt32(this.ComboBoxOddeleni.SelectedValue);
                ulozitBodAP.SkutecnaPricinaWM = string.IsNullOrEmpty(this.RichTextBoxSkutecnaPricinaWM.Text)
                    ? null
                    : this.RichTextBoxSkutecnaPricinaWM.Text;
                ulozitBodAP.NapravnaOpatreniWM = string.IsNullOrEmpty(this.RichTextBoxNapravnaOpatreniWM.Text)
                    ? null
                    : this.RichTextBoxNapravnaOpatreniWM.Text;
                ulozitBodAP.SkutecnaPricinaWS = string.IsNullOrEmpty(this.RichTextBoxSkutecnaPricinaWS.Text)
                    ? null
                    : this.RichTextBoxSkutecnaPricinaWS.Text;
                ulozitBodAP.NapravnaOpatreniWS = string.IsNullOrEmpty(this.RichTextBoxNapravnaOpatreniWS.Text)
                    ? null
                    : this.RichTextBoxNapravnaOpatreniWS.Text;
            }

            //vytvoření nebo aktualizace nového bodu
            var bodAPId = BodAPDataMapper.InsertUpdateBodAP(ulozitBodAP);

            _ = MessageBox.Show("The AP Point has been saved.", "Notice", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            //při uložení nového bodu je přepsána proměnná novyBodAP na false, protože dále mohu editovat tento bod jako již uložený
            if (this.novyBodAP)
            {
                this.novyBodAP = false;
                this.cisloRadkyDGVBody = FormPrehledBoduAP.bodyAP.Count - 1;
                FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].Id = bodAPId;
                FormPrehledBoduAP.bodyAP[this.cisloRadkyDGVBody].BodUlozen = true;
            }

            this.ZobrazeniDGV();
        }
    }
}
