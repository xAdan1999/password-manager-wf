﻿using password_manager_wf.Models.Responses;
using password_manager_wf.Views.Modals;
using password_manager_wf.Controlles;
using password_manager_wf.Models;
using password_manager_wf.Cache;
using System.Collections.Generic;
using System.Windows.Forms;
using System;

namespace password_manager_wf.Views
{
    public partial class MainApp : Form
    {
        //para hacer peticiones a la API
        PasswordService passwordService = new PasswordService();

        //para hacer busquedas
        Search search = new Search();

        public MainApp()
        {
            InitializeComponent();
        }

        private async void MainApp_Load(object sender, EventArgs e)
        {
            GetUserInfo();

            List<PasswordResponse> passwords = await passwordService.GetPasswords();

            if (passwords != null)
            {
                dataGridView1.RowTemplate.Height = 50;
                dataGridView1.DataSource = passwords;
                SetDataGridViewStyle();
                CountItems();
            }
        }

        private void GetUserInfo()
        {
            lb_username.Text = string.Format("Welcome! {0}", UserCache.username);
        }

        private async void GetPasswords()
        {
            List<PasswordResponse> passwords = await passwordService.GetPasswords();
            dataGridView1.DataSource = passwords;
            CountItems();
        }

        private void SetDataGridViewStyle()
        {
            dataGridView1.Columns[0].DisplayIndex = 5;
            dataGridView1.Columns[1].DisplayIndex = 5;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].HeaderText = "Title";
            dataGridView1.Columns[4].HeaderText = "Username or email";
            dataGridView1.Columns[4].Width = 350;
            dataGridView1.Columns[5].HeaderText = "Password";
            dataGridView1.Columns[5].Width = 450;
        }

        private async void SearchPassword()
        {
            search.search = txt_search.Text.Trim();
            List<PasswordResponse> passwords = await passwordService.SearchPassword(search);
            dataGridView1.DataSource = passwords;
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            SearchPassword();
        }

        private void btn_addPassword_Click(object sender, EventArgs e)
        {
            PasswordModal modal = new PasswordModal();
            var result = modal.ShowDialog();
            if (result == DialogResult.OK)
                GetPasswords();
        }

        private async void DeletePassword(int id)
        {
            bool succes = await passwordService.DeletePassword(id);
            if (succes) 
                GetPasswords();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[0].Selected)
                {
                    int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                    PasswordModal modal = new PasswordModal(id, true);
                    modal.txt_title.Text = dataGridView1.Rows[e.RowIndex].Cells["title"].Value.ToString().Trim();
                    modal.txt_usernameOrEmail.Text = dataGridView1.Rows[e.RowIndex].Cells["usernameOrEmail"].Value.ToString().Trim();
                    modal.txt_password.Text = dataGridView1.Rows[e.RowIndex].Cells["passwordToSave"].Value.ToString().Trim();
                    var result = modal.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        GetPasswords();
                    }
                }
                else if (dataGridView1.Rows[e.RowIndex].Cells[1].Selected)
                {
                    int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                    DeletePassword(id);
                }
            }
        }

        private void CountItems()
        {
            int items = dataGridView1.Rows.Count;
            lb_countItems.Text = string.Format($"items: {items}");
        }
    }
}
