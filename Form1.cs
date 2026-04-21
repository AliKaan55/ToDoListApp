using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ToDoListAPP;
using static ToDoListAPP.Form1;


namespace ToDoListAPP
{
    public partial class Form1 : Form
    {
        private List<TodoItem> todoItems;
        public Form1()
        {
            InitializeComponent();
            todoItems = new List<TodoItem>();
            InitializeCustomSettings();
        }
        private void ListBoxTasks_ItemCheck(object sender, ItemCheckEventArgs e)
        {
          
            this.BeginInvoke(new Action(() =>
            {
                if (e.Index >= 0 && e.Index < todoItems.Count)
                {
                    todoItems[e.Index].IsCompleted = listBoxTasks.GetItemChecked(e.Index);
                    UpdateStatistics();
                }
            }));
        }
        private void AddTask()
        {
            string task = txtTask.Text.Trim();

            if (string.IsNullOrEmpty(task))
            {
                MessageBox.Show("Lütfen bir görev girin!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            TodoItem newItem = new TodoItem
            {
                Task = task,
                IsCompleted = false,
                CreatedDate = DateTime.Now
            };

            todoItems.Add(newItem);
            listBoxTasks.Items.Add(task);

            txtTask.Clear();
            txtTask.Focus();
            UpdateStatistics();
        }
        private void TxtTask_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (e.KeyChar == (char)Keys.Enter)
            {
                AddTask();
                e.Handled = true;
            }
        }
        private void InitializeCustomSettings()
        {
          
            listBoxTasks.CheckOnClick = true;
            listBoxTasks.ItemCheck += ListBoxTasks_ItemCheck;
            
            txtTask.KeyPress += TxtTask_KeyPress;
            UpdateStatistics();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddTask();
        }
        private void btnComplete_Click(object sender, EventArgs e)
        {
            if (listBoxTasks.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen bir görev seçin!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int index = listBoxTasks.SelectedIndex;
            bool currentState = listBoxTasks.GetItemChecked(index);
            listBoxTasks.SetItemChecked(index, !currentState);
            todoItems[index].IsCompleted = !currentState;

            UpdateStatistics();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxTasks.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen silinecek görevi seçin!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Seçili görevi silmek istediğinizden emin misiniz?",
                "Onay",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int index = listBoxTasks.SelectedIndex;
                todoItems.RemoveAt(index);
                listBoxTasks.Items.RemoveAt(index);
                UpdateStatistics();
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (listBoxTasks.Items.Count == 0)
            {
                MessageBox.Show("Liste zaten boş!", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Tüm görevleri silmek istediğinizden emin misiniz?",
                "Onay",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                listBoxTasks.Items.Clear();
                todoItems.Clear();
                UpdateStatistics();
                txtTask.Clear();
            }
        }
        private void UpdateStatistics()
        {
            int total = todoItems.Count;
            int completed = todoItems.FindAll(x => x.IsCompleted).Count;
            int remaining = total - completed;

            lblStats.Text = $"Toplam: {total}  |  Tamamlanan: {completed}  |  Kalan: {remaining}";
        }
        public class TodoItem
        {
            public string Task { get; set; }
            public bool IsCompleted { get; set; }
            public DateTime CreatedDate { get; set; }
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
