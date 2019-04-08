using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    private string patientXmlFile = $"{AppDomain.CurrentDomain.BaseDirectory}patients.xml";
    private XElement patients;

    protected void Page_Load(object sender, EventArgs e)
    {
        txtFirstName.Attributes.Add("required", "");
        txtLastName.Attributes.Add("required", "");
        divError.Visible = false;

        if (!File.Exists(patientXmlFile))
        {
            var xmlWriter = new XmlTextWriter(patientXmlFile, Encoding.UTF8);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("patients");
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }

        patients = XElement.Load(patientXmlFile);

        LoadPatients();
    }

    private void LoadPatients()
    {
        tblPatient.Rows.Clear();

        var th = new TableHeaderRow();
        th.Cells.Add(new TableHeaderCell() { Text = "First Name" });
        th.Cells.Add(new TableHeaderCell() { Text = "Last Name" });
        th.Cells.Add(new TableHeaderCell() { Text = "Gender" });
        th.Cells.Add(new TableHeaderCell() { Text = "Phone" });
        th.Cells.Add(new TableHeaderCell() { Text = "Email" });
        th.Cells.Add(new TableHeaderCell() { Text = "Notes" });
        th.Cells.Add(new TableHeaderCell() { Text = "" });
        th.Cells.Add(new TableHeaderCell() { Text = "" });
        tblPatient.Rows.Add(th);

        var patientId = 0;

        patients.Descendants("patient")
            .Where(xl => !bool.Parse(xl.Element("is-deleted").Value))
            .ToList()
            .ForEach(xl =>
            {
                patientId++;

                var tr = new TableRow();
                tr.Cells.Add(new TableCell() { Text = xl.Element("first-name").Value });
                tr.Cells.Add(new TableCell() { Text = xl.Element("last-name").Value });
                tr.Cells.Add(new TableCell() { Text = xl.Element("gender").Value });
                tr.Cells.Add(new TableCell() { Text = xl.Element("phone").Value });
                tr.Cells.Add(new TableCell() { Text = xl.Element("email").Value });
                tr.Cells.Add(new TableCell() { Text = xl.Element("notes").Value });

                var tdEdit = new TableCell() { Width = Unit.Percentage(5) };
                var lnkEdit = new LinkButton()
                {
                    ID = $"editPatient{patientId}",
                    Text = "Edit"
                };
                lnkEdit.Command += lnkEdit_Click;
                lnkEdit.CommandArgument = patientId.ToString();

                tdEdit.Controls.Add(lnkEdit);
                tr.Cells.Add(tdEdit);

                var tdDelete = new TableCell() { Width = Unit.Percentage(5) };
                var lnkDelete = new LinkButton()
                {
                    ID = $"delPatient{patientId}",
                    Text = "Delete"
                };
                lnkDelete.Command += lnkDelete_Click;
                lnkDelete.CommandArgument = patientId.ToString();
                
                tdDelete.Controls.Add(lnkDelete);
                tr.Cells.Add(tdDelete);
                
                tblPatient.Rows.Add(tr);
            });
    }

    private XElement FindPatientByName(string firstName, string lastName)
    {
        return patients.Descendants("patient")
            .Where(p => !bool.Parse(p.Element("is-deleted").Value))
            .FirstOrDefault(p => 
                p.Element("first-name").Value.Equals(firstName, StringComparison.OrdinalIgnoreCase) && 
                p.Element("last-name").Value.Equals(lastName, StringComparison.OrdinalIgnoreCase));
    }

    private XElement FindPatientById(int id)
    {
        return patients.Descendants("patient")
            .Where(p => !bool.Parse(p.Element("is-deleted").Value))
            .Select((item, index) => new { item, id = index + 1 })
            .FirstOrDefault(p => p.id == id)?.item;
    }

    private void ClearInput()
    {
        hfPatientId.Value = "0";
        txtFirstName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        ddlGender.SelectedIndex = 0;
        txtEmail.Text = string.Empty;
        txtPhone.Text = string.Empty;
        txtNotes.Text = string.Empty;
        divError.Visible = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        var patientId = Convert.ToInt32(hfPatientId.Value);
        XElement patient = null;

        if (patientId > 0)
        {
            patient = FindPatientById(patientId);
            patient.Element("first-name").Value = txtFirstName.Text;
            patient.Element("last-name").Value = txtLastName.Text;
            patient.Element("gender").Value = ddlGender.SelectedValue;
            patient.Element("email").Value = txtEmail.Text;
            patient.Element("phone").Value = txtPhone.Text;
            patient.Element("notes").Value = txtNotes.Text;
            patient.Element("last-updated-date").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt");
        }
        else
        {
            patient = FindPatientByName(txtFirstName.Text, txtLastName.Text);
            if (patient != null)
            {
                divError.Visible = true;
                return;
            }

            patient = new XElement("patient",
                new XElement("first-name", txtFirstName.Text),
                new XElement("last-name", txtLastName.Text),
                new XElement("gender", ddlGender.SelectedValue),
                new XElement("email", txtEmail.Text),
                new XElement("phone", txtPhone.Text),
                new XElement("notes", txtNotes.Text),
                new XElement("is-deleted", bool.FalseString),
                new XElement("date-created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt")),
                new XElement("last-updated-date", string.Empty)
            );

            patients.Add(patient);
        }

        patients.Save(patientXmlFile);

        LoadPatients();
        ClearInput();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearInput();
    }

    protected void lnkEdit_Click(object sender, CommandEventArgs e)
    {
        var patientId = Convert.ToInt32(e.CommandArgument);
        var patient = FindPatientById(patientId);

        ClearInput();

        hfPatientId.Value = patientId.ToString();
        txtFirstName.Text = patient.Element("first-name").Value;
        txtLastName.Text = patient.Element("last-name").Value;
        ddlGender.SelectedValue = patient.Element("gender").Value;
        txtEmail.Text = patient.Element("email").Value;
        txtPhone.Text = patient.Element("phone").Value;
        txtNotes.Text = patient.Element("notes").Value;
    }

    protected void lnkDelete_Click(object sender, CommandEventArgs e)
    {
        var patient = FindPatientById(Convert.ToInt32(e.CommandArgument));

        if (patient != null)
        {
            patient.Element("is-deleted").Value = bool.TrueString;
            patients.Save(patientXmlFile);

            LoadPatients();
        }
    }
}