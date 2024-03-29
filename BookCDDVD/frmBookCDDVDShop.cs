﻿
/*
 * 
 * 
 * Tyler Doudrick
 * Tai Nguyen
 * 11/23/2019
 * Codebehind for Main Form
 * Project 4: BookCDDVDShop Stage II
 * 
 * 
 * ******NOTE TO GRADERS******
 * 
 * This Project doesn't require classes for the different types of products now that the database has been introduced.
 *          This fact was confirmed with Frank, so now our program does not use product classes or their children.
 *          Instead, the database handler was rewritten, and everything is handled using dictionaries.
 *          
 *          This saves the useless step of grabbing information from the DB, building an object from it,
 *          and then immediately destructuring the object to add its contents to the form.
 *          
 *          The DB Handler was rewritten to be more efficient using transactions where possible instead
 *          of opening and reopening the SQL connections, and SQL connections are opened/closed via the 
 *          'using' keyword.
 *          
 *          Methods to insert a product have been condensed into one, making it much more palatable. 
 * 
 * Anyway, we have permission from Frank to do it this way due to ambigious requirements at the start.
 * 
 * Cheers :)
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BookCDDVD
{
    public partial class frmBookCDDVDShop : Form
    {
        private List<GroupBox> groupList = new List<GroupBox>(6);
        private List<Button> buttonList = new List<Button>(5);

        private ProductDB dbProducts = new ProductDB();

        private bool editingTrigger = false;
        private int foundUPC = 0;
        private string foundType = "";


        public frmBookCDDVDShop()
        {
            InitializeComponent();

            // adding all the groups into the groupList
            groupList.Add(grpBook);
            groupList.Add(grpBookCIS);
            groupList.Add(grpCDChamber);
            groupList.Add(grpCDClassical);
            groupList.Add(grpCDOrchestra);
            groupList.Add(grpDVD);

            // adding all the new entry button into the buttonList
            buttonList.Add(btnCreateBook);
            buttonList.Add(btnCreateBookCIS);
            buttonList.Add(btnCreateDVD);
            buttonList.Add(btnCreateCDOrchestra);
            buttonList.Add(btnCreateCDChamber);
            buttonList.Add(btnCreateCDOrchestra);


            // adding options into the CIS Area dropdown
            cbBookCISArea.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBookCISArea.Items.Add("Computer Sciences");
            cbBookCISArea.Items.Add("Information Sciences");

        } // end frmBookCDDVDShop



        // this method handles some stuff that happens when the form loads
        private void frmBookCDDVDShop_Load(object sender, EventArgs e)
        {
            // hide the textboxes
            grpProduct.Visible = false;

            //Set the DVD Release Datepicker to today.
            dtDVDReleaseDate.MaxDate = DateTime.Today;

            //Initialize the control tooltips
            setToolTips();

            lblUniqProducts.Text = dbProducts.getRowCount().ToString();


            dbProducts.deleteProduct(12345, "CDOrchestra");
            dbProducts.deleteProduct(12345, "CDClassical");


        } // end frmBookCDDVDShop_Load


          // this method show a message at the bottom of textboxes to give the users what they should enter the textboxes
        private void setToolTips()
        {
            // setting value to the tooltips 
            ToolTip tip = new ToolTip();

            tip.ShowAlways = true;
            // setting a specific values to textboxes and button on the form
            tip.SetToolTip(this.txtProductUPC, "Enter a 5 digits UPC");
            tip.SetToolTip(this.txtProductPrice, "Enter a price, decimal will be rounded up to 2 decimals");
            tip.SetToolTip(this.txtProductTitle, "Enter the book title");
            tip.SetToolTip(this.txtProductQuantity, "Enter the quantity");
            tip.SetToolTip(this.txtBookISBNLeft, "Enter the first 3 digits of ISBN");
            tip.SetToolTip(this.txtBookISBNRight, "Enter the last 3 digits of ISBN");
            tip.SetToolTip(this.txtBookAuthor, "Enter the Author name, Number are not allowed");
            tip.SetToolTip(this.txtBookPages, "Enter a number of pages, cannot be more then 9999");
            tip.SetToolTip(this.txtDVDLeadActor, "Enter the Lead Actor name");
            tip.SetToolTip(this.dtDVDReleaseDate, "Choose a date");
            tip.SetToolTip(this.cbBookCISArea, "Choose a CIS area");
            tip.SetToolTip(this.txtDVDRunTime, "enter a run time that cannot be less then 0 or more than 120 minutes");
            tip.SetToolTip(this.txtCDClassicalLabel, "Enter a label name");
            tip.SetToolTip(this.txtCDClassicalArtists, "Enter the Artist name");
            tip.SetToolTip(this.txtCDOrchestraConductor, "Enter the Conductor name");
            tip.SetToolTip(this.btnInsert, "Add the inputed information into the data entry");
            tip.SetToolTip(this.btnDelete, "Delete the information");
            tip.SetToolTip(this.btnClear, "Clear the textboxes on the form");
            tip.SetToolTip(this.btnExit, "Exit the program");
        } // end setToolTips

        // this method hide and enable textboxes for Create Book button
        private void btnCreateBook_Click(object sender, EventArgs e)
        {
            // hide every other group that are not CreateBook
            Validator.hideGroups(grpBook, groupList, btnDelete, grpProduct);

            // disable CreateaBook button and enable all other new entry button
            Validator.disableCreateButton(btnCreateBook, buttonList);

            // clear the form
            clearForm();
        } // end btnCreateaBook

        // this method hide and enable textboxes for Create Book CIS button
        private void btnCreateBookCIS_Click(object sender, EventArgs e)
        {
            // hide every other group that are not BookCIS and Book
            Validator.hideGroups(grpBook, grpBookCIS, groupList, btnDelete, grpProduct);

            // disable CreateaBookCIS button and enable all other new entry button
            Validator.disableCreateButton(btnCreateBookCIS, buttonList);

            // clear the form
            clearForm();
        } // end btnCreateaBookCIS

        // this method hide and enable textboxes for Create DVD button
        private void btnCreateDVD_Click(object sender, EventArgs e)
        {
            // hide every group that is not DVD
            Validator.hideGroups(grpDVD, groupList, btnDelete, grpProduct);

            // disable btnCreateDVD and enable all other new entry button
            Validator.disableCreateButton(btnCreateDVD, buttonList);

            // clear the form
            clearForm();
        } // end btnCreateaDVD

        // this method only show and enable textboxes for createa DVD Orchestra button
        private void btnCreateCDOrchestra_Click(object sender, EventArgs e)
        {
            // hide every group that are not CDOrchestra and CDClassical
            Validator.hideGroups(grpCDClassical, grpCDOrchestra, groupList, btnDelete, grpProduct);

            // disabe createaDVDOrchestra button and enable other new entry button
            Validator.disableCreateButton(btnCreateCDOrchestra, buttonList);

            // clear the form
            clearForm();
        } // end btnCreateaCDOrchestra

        // this method only show and enable textboxes for createa CD Chamber button
        private void btnCreateCDChamber_Click(object sender, EventArgs e)
        {
            // hide every group that is not related to CD chamber
            Validator.hideGroups(grpCDChamber, grpCDClassical, groupList, btnDelete, grpProduct);

            // disable button createa CD Chamber and enable all other new entry button
            Validator.disableCreateButton(btnCreateCDChamber, buttonList);

            // clear the form
            clearForm();
        } // end btnCreateCDChamber

        // this method clear all of the textboxes
        private void btnClear_Click(object sender, EventArgs e)
        {
            // clear the form and reset everything 
            clearForm();
        } // end btnClear_Click


        // this method clear all the textboxes
        private void clearForm()
        {
            // clearing the textboxes
            txtProductUPC.Clear();
            txtProductPrice.Clear();
            txtProductTitle.Clear();
            txtProductQuantity.Clear();
            txtBookISBNLeft.Clear();
            txtBookISBNRight.Clear();
            txtBookAuthor.Clear();
            txtBookPages.Clear();
            cbBookCISArea.ResetText();
            txtDVDLeadActor.Clear();
            txtDVDRunTime.Clear();
            txtCDClassicalLabel.Clear();
            txtCDClassicalArtists.Clear();
            txtCDOrchestraConductor.Clear();
            txtCDChamberInstrumentList.Text = "";
            cbBookCISArea.SelectedIndex = -1;
            txtProductUPC.Focus();

            //Reset the Release Date picker to the current date
            dtDVDReleaseDate.Value = DateTime.Today;

            //Reset the editiong/delete controls/variables
            btnDelete.Visible = false;
            editingTrigger = false;
            btnInsert.Text = "Insert";
            txtProductUPC.ReadOnly = false;

        } // end clearForm

        // this method find which groupbox are visible and Validator those textboxes input
        private void ValidatorVisibleGrp()
        {
            //Dictionary to hold the parameters we'll be passing in
            //Basically using an associative array that we can pass attributes to as needed for each type of product
            IDictionary<string, string> dict = new Dictionary<string, string>();

            //First, check if the product textboxes are valid, since all types of products have these fields
            //Set those fields in the dictionary

            //
            //The product type is determined by what groups are visible
            //The product type is used heavily to determine what parameters are placed into
            //     or read from the dictionary
            //

            if (Validator.checkProduct(txtProductUPC, txtProductPrice, txtProductTitle, txtProductQuantity))
            {
                dict["fldUPC"] = txtProductUPC.Text;
                dict["fldPrice"] = txtProductPrice.Text;
                dict["fldTitle"] = txtProductTitle.Text;
                dict["fldQuantity"] = txtProductQuantity.Text;

                // if groupbox book are visible or groupbox book and bookCIS are visible, check the input validation
                //After checking validation, set the values in the dictionary. Then create the product.

                if (Validator.checkVisibleGroup(grpBook))
                {
                    if (Validator.checkBook(txtBookISBNLeft, txtBookISBNRight, txtBookAuthor, txtBookPages))
                    {
                        dict["fldISBN"] = txtBookISBNLeft.Text + txtBookISBNRight.Text;
                        dict["fldAuthor"] = txtBookAuthor.Text;
                        dict["fldPages"] = txtBookPages.Text;

                        if (Validator.checkVisibleGroup(grpBookCIS))
                        {
                            dict["fldCISArea"] = cbBookCISArea.Text;
                            createProduct("BookCIS", dict);
                        }
                        else
                        {
                            createProduct("Book", dict);
                        }
                    }
                }

                // if groupbox DVD are visible, check the input validation
                //After checking validation, set the values in the dictionary. Then create the product.

                else if (Validator.checkVisibleGroup(grpDVD))
                {
                    if (Validator.checkDVD(txtDVDLeadActor, txtDVDRunTime))
                    {
                        dict["fldLeadActor"] = txtDVDLeadActor.Text;
                        dict["fldReleaseDate"] = dtDVDReleaseDate.Value.ToShortDateString();
                        dict["fldRunTime"] = txtDVDRunTime.Text;
                        createProduct("DVD", dict);
                    }
                }
                // if groupbox CD Classical are visisble, check the input validation
                //After checking validation, set the values in the dictionary. Then create the product.

                else if (Validator.checkVisibleGroup(grpCDChamber))
                {
                    if (Validator.checkCDClassical(txtCDClassicalLabel, txtCDClassicalArtists))
                    {
                        dict["fldLabel"] = txtCDClassicalLabel.Text;
                        dict["fldArtists"] = txtCDClassicalArtists.Text;
                        dict["fldInstrumentList"] = txtCDChamberInstrumentList.Text;
                        createProduct("CDChamber", dict);
                    }
                }
                // if groupbox CD Orchestra are visible, check the input validation
                //After checking validation, set the values in the dictionary. Then create the product.

                else if (Validator.checkVisibleGroup(grpCDOrchestra))
                {
                    if (Validator.checkOrchestralConductor(txtCDOrchestraConductor))
                    {
                        dict["fldLabel"] = txtCDClassicalLabel.Text;
                        dict["fldArtists"] = txtCDClassicalArtists.Text;
                        dict["fldConductor"] = txtCDOrchestraConductor.Text;
                        createProduct("CDOrchestra", dict);
                    }
                }
            }
        } // end ValidatorVisibleGrp

        private void createProduct(string type, IDictionary<string, string> param)
        {
            //The type of product is passed in as a string for the case/switch statement
            //If we're not editing, we're inserting a product
            string outString = "";
            if (!editingTrigger)
            {
                if (dbProducts.InsertProduct(type, param, ref outString))
                {
                    MessageBox.Show(outString, "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    lblUniqProducts.Text = (Int32.Parse(lblUniqProducts.Text) + 1).ToString();

                    //Enable the search button in the event that there were previously no items in the db.
                    btnSearch.Enabled = true;

                }
                else MessageBox.Show(outString, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            //if we're editing, we're updating the product
            else
            {
                //The update button was pressed!, use the updateProduct method to send the UPDATE statements to the DB.

                dbProducts.updateProduct(foundUPC, param, type);

                //Hide the delete button since they're no longer editing
                btnDelete.Visible = false;

                MessageBox.Show("Updated Product Information!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //clear the form
            clearForm();
        }

        // this button will insert product into the database after being click
        private void btnInsert_Click(object sender, EventArgs e)
        {
            // Validator the visible groupbox
            ValidatorVisibleGrp();
        } // end btnInsert_Click

        // this method exit the program
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        } // end btnExit_Click

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Show the search box when the search button is clicked
            frmSearchDialog SearchDialog = new frmSearchDialog();

            int enteredUPC = 0;

            //The searchbox will return a dialogresult of OK when the search button within it is clicked
            DialogResult result = DialogResult.Retry;
            while (result == DialogResult.Retry)
            {
                try
                {
                    if (SearchDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        //Check the returnedUPC within the dialog box; That's what the user entered.
                        if (SearchDialog.getUPC().ToString().Length == 5 && Int32.TryParse(SearchDialog.getUPC(), out enteredUPC))
                        {

                            if (dbProducts.getProduct(enteredUPC, out string type, out IDictionary<string, string> outDict))
                            {

                                //Import the data from the Product table into the form
                                txtProductUPC.Text = enteredUPC.ToString();
                                txtProductTitle.Text = outDict["fldTitle"];
                                txtProductQuantity.Text = outDict["fldQuantity"];
                                txtProductPrice.Text = outDict["fldPrice"];

                                foundUPC = enteredUPC;
                                foundType = type;

                                //Switch/Case for each type of product
                                switch (type)
                                {
                                    //For each respective type, we show the groups that correspond to that product type.
                                    //Then we pull the product's information from the returned dictionary
                                    //      depending on what type of product it is

                                    case "Book":
                                        Validator.hideGroups(grpBook, groupList, btnDelete, grpProduct);
                                        txtBookAuthor.Text = outDict["fldAuthor"];
                                        txtBookISBNLeft.Text = outDict["fldISBN"].Substring(0, 3);
                                        txtBookISBNRight.Text = outDict["fldISBN"].Substring(3);
                                        txtBookPages.Text = outDict["fldPages"];
                                        break;
                                    case "BookCIS":
                                        Validator.hideGroups(grpBook, grpBookCIS, groupList, btnDelete, grpProduct);
                                        txtBookAuthor.Text = outDict["fldAuthor"];
                                        txtBookISBNLeft.Text = outDict["fldISBN"].Substring(0, 3);
                                        txtBookISBNRight.Text = outDict["fldISBN"].Substring(3);
                                        MessageBox.Show(outDict["fldISBN"]);
                                        txtBookPages.Text = outDict["fldPages"];
                                        cbBookCISArea.SelectedIndex = cbBookCISArea.FindStringExact(outDict["fldCISArea"]);
                                        
                                        break;
                                    case "DVD":
                                        Validator.hideGroups(grpDVD, groupList, btnDelete, grpProduct);
                                        txtDVDLeadActor.Text = outDict["fldLeadActor"];
                                        txtDVDRunTime.Text = outDict["fldRunTime"];

                                        outDict["fldReleaseDate"] = Convert.ToDateTime(outDict["fldReleaseDate"]).ToString("dd/MM/yyyy");
                                        dtDVDReleaseDate.Value = DateTime.ParseExact(outDict["fldReleaseDate"], "dd/MM/yyyy", null);
                                        break;
                                    case "CDOrchestra":
                                        Validator.hideGroups(grpCDClassical, grpCDOrchestra, groupList, btnDelete, grpProduct);
                                        txtCDClassicalLabel.Text = outDict["fldLabel"];
                                        txtCDClassicalArtists.Text = outDict["fldArtists"];
                                        txtCDOrchestraConductor.Text = outDict["fldConductor"];

                                        break;
                                    case "CDChamber":
                                        Validator.hideGroups(grpCDClassical, grpCDChamber, groupList, btnDelete, grpProduct);
                                        txtCDClassicalLabel.Text = outDict["fldLabel"];
                                        txtCDClassicalArtists.Text = outDict["fldArtists"];
                                        txtCDChamberInstrumentList.Text = outDict["fldInstrumentList"];
                                        break;
                                } // end switch
                                //Show the delete button and set the trigger for editing
                                //
                                //
                                //      The insert button now functions as an update button until either:
                                //          1. The Product is updated
                                //          2. The Form is cleared/A create button is pressed
                                //          3. The Product is deleted
                                //

                                btnDelete.Visible = true;
                                editingTrigger = true;
                                txtProductUPC.ReadOnly = true;
                                btnInsert.Text = "Update";
                            }
                            else
                            {
                                throw new Exception("The UPC was not found in the Database");
                            } // end else
                        } // end if
                        else
                        {
                            throw new Exception("Invalid UPC Was Entered.");
                        } // end else
                    } // 
                    else
                    {
                        throw new Exception("Search Cancelled.");
                    } // end else
                    break;
                } // end try
                catch(Exception ex)
                {
                    result = MessageBox.Show(ex.Message, "Search Failed!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);

                    if (result == DialogResult.Cancel) break;
                } // end catch
            } // end while
            //Dispose of the search dialog since we're done with it
            SearchDialog.Dispose();
        } // end btnSearch_Click

        // this button delete a product from the database
        private void btnDelete_Click(object sender, EventArgs e)
        {
            clearForm();
            //Remove one product from the number of products we have using the return from the removeProduct method
            //lblUniqProducts.Text = (InStock.removeProduct(indexToDelete).ToString());

            //Remove the product from the DB
            if(dbProducts.deleteProduct(foundUPC, foundType))
            {
                lblUniqProducts.Text = (Int32.Parse(lblUniqProducts.Text) - 1).ToString();

                MessageBox.Show("Product removed from inventory. There are now " + lblUniqProducts.Text +" products in inventory.");

                //Reset the upc value found before
                foundUPC = 0;

                //Reset the type string found before

                foundType = "";
                //Hide the delete button
                btnDelete.Visible = false;

            }
        } // end btnDelete_Click
    } // end frmBookCDDVDShop
} // end namespace BookCDDVD