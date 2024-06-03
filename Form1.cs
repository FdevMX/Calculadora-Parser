using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.calitha.goldparser;

namespace CalculadoraExamen
{
    public partial class Form1 : Form
    {
        MyParser parser;
        public Form1()
        {
            parser = new MyParser(Application.StartupPath + "\\gramatica.cgt");
            InitializeComponent();
        }

        //Variables
        bool varClean = false, funcParentesis = false;
        private bool panel1Colapsado = false;
        private Bitmap imgExpand = Properties.Resources.back;
        private Bitmap imgColaps = Properties.Resources.front;
        private string modo = "deg"; // Modo inicial es RAD

        Panel pnHis, pnMem;
        Label lbl1His, lbl2His, lbl1Mem;
        int nHis = 1, nMem = 1;
        Label[] lb1His = new Label[0], lb2His = new Label[0], lb1Mem = new Label[0];

        private void Form1_Load(object sender, EventArgs e)
        {
            btnColapse.Image = imgExpand;
            // En el evento Load del formulario o en el constructor
            txtOperacion.AcceptsReturn = true; // Para permitir ingresar saltos de línea
            txtOperacion.ImeMode = ImeMode.Alpha; // Modo de entrada de texto sin formato
        }

        private void btnColapse_Click(object sender, EventArgs e)
        {
            // Alternamos el valor de la variable panel1Colapsado
            panel1Colapsado = !panel1Colapsado;
            PanelControls.Panel1Collapsed = panel1Colapsado;

            //Cambiar el icono del boton
            if (panel1Colapsado)
            {
                btnColapse.Image = imgColaps;
            }
            else
            {
                btnColapse.Image = imgExpand;
            }
        }

        private void LimpiarResultado()
        {
            if (txtResultado.Text != "")
            {
                txtOperacion.Text = "";
                txtResultado.Text = "";
            }
        }

        private double Evaluate(string expression)
        {
            expression = expression.Replace(",", ".");
            expression = expression.Replace("÷", "/");
            expression = expression.Replace("×", "*");
            try
            {
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("expression", string.Empty.GetType(), expression);
                System.Data.DataRow row = table.NewRow();
                table.Rows.Add(row);
                return double.Parse((string)row["expression"]);
            }
            catch
            {
                return double.Parse("0");
            }
        }



        #region MEMORIA

        // Funcion de historial
        private void Historial(string num, string men)
        {
            pnHis = new Panel();
            lbl1His = new Label();
            lbl2His = new Label();

            pnHis.Height = 70;
            pnHis.Dock = DockStyle.Top;
            pnHis.MouseEnter += new EventHandler(pnFoco);
            pnHis.MouseLeave += new EventHandler(pnForaFoco);
            pnHis.Click += new EventHandler(pnClickHis);
            pnHis.Name = "pn" + nHis;

            lbl1His.Text = men + " =";
            lbl1His.Font = new Font("Product Sans", 15);
            lbl1His.Name = "lbl1_" + nHis;
            lbl1His.Left = panelHistorial.Width - lbl1His.Width;

            lbl2His.Text = num;
            lbl2His.Font = new Font("Product Sans", 22, FontStyle.Bold);
            lbl2His.Top = 20;
            lbl2His.Name = "lbl2_" + nHis;
            lbl2His.Height = 32;
            lbl2His.Left = panelHistorial.Width - lbl1His.Width;

            panelHistorial.Controls.Add(pnHis);
            pnHis.Controls.Add(lbl2His);
            pnHis.Controls.Add(lbl1His);

            Array.Resize(ref lb1His, nHis);
            Array.Resize(ref lb2His, nHis);
            lb1His[nHis - 1] = lbl1His;
            lb2His[nHis - 1] = lbl2His;
            nHis++;

        }

        private void pnFoco(object sender, EventArgs e)
        {
            Panel pn = sender as Panel;
            pn.BackColor = Color.Silver;
        }

        private void pnForaFoco(object sender, EventArgs e)
        {
            Panel pn = sender as Panel;
            pn.BackColor = Color.Transparent;
        }

        private void pnClickHis(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            string a = panel.Name.Remove(0, 2);
            lbl1His = lb1His[int.Parse(a) - 1];
            lbl2His = lb2His[int.Parse(a) - 1];

            txtResultado.Text = lbl2His.Text;
            txtOperacion.Text = lbl1His.Text.Remove(lbl1His.Text.Length - 2, 2);

            varClean = true;
            funcParentesis = false;
        }

        // Funcion de memoria
        private void Memoria(string num)
        {
            pnMem = new Panel();
            lbl1Mem = new Label();


            pnMem.Height = 70;
            pnMem.Dock = DockStyle.Top;
            pnMem.MouseEnter += new EventHandler(pnFoco);
            pnMem.MouseLeave += new EventHandler(pnForaFoco);
            pnMem.Click += new EventHandler(pnClickMem);
            pnMem.Name = "pn" + nMem;


            lbl1Mem.Text = num;
            lbl1Mem.Font = new Font("Product Sans", 18, FontStyle.Bold);
            lbl1Mem.Top = 20;
            lbl1Mem.Name = "lbl2_" + nMem;
            lbl1Mem.Height = 32;
            lbl1Mem.Left = panelMemoria.Width - lbl1Mem.Width;

            panelMemoria.Controls.Add(pnMem);
            pnMem.Controls.Add(lbl1Mem);

            Array.Resize(ref lb1Mem, nMem);
            lb1Mem[nMem - 1] = lbl1Mem;
            nMem++;

        }

        private void pnClickMem(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            string a = panel.Name.Remove(0, 2);
            lbl1Mem = lb1Mem[int.Parse(a) - 1];

            txtResultado.Text = lbl1Mem.Text;

            varClean = true;
            funcParentesis = false;
        }


        #endregion

        #region BOTONESPANEL2
        private void btnOne_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "1";
        }

        private void btnTwo_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "2";
        }

        private void btnThree_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "3";
        }

        private void btnFour_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "4";
        }

        private void btnFive_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "5";
        }

        private void btnSix_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "6";
        }

        private void btnSeven_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "7";
        }

        private void btnEight_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "8";
        }

        private void btnNine_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "9";
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "0";
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += ".";
        }

        private void btnSum_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "+";
        }

        private void btnRes_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "-";
        }

        private void btnMul_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "*";
        }

        private void btnDiv_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "/";
        }

        private void btnExponente_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "**";
        }

        private void btnPorcent_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "%";
        }
        #endregion


        #region BOTONESPANEL1
        private void btnParentOpen_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "(";
        }

        private void btnParentClose_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += ")";
        }

        private void btnSin_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "sin(";
        }

        private void btnCos_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "cos(";
        }

        private void btnTan_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "tan(";
        }

        private void btnRaiz_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "sqrt(";
        }

        private void btnPi_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += Math.PI.ToString();
        }

        private void btnEuler_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "exp^";
        }

        private void btnFactorial_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "!";
        }

        private void btnXala2_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "^2";
        }

        private void btnUnoEntreX_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "1/";
        }

        private void btnLn_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += "ln(";
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            txtOperacion.Text += ",";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtOperacion.Clear();
            txtResultado.Clear();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (txtOperacion.Text.Length > 0)
            {
                txtOperacion.Text = txtOperacion.Text.Remove(txtOperacion.Text.Length - 1);
            }
        }

        private void btnMS_Click(object sender, EventArgs e)
        {
            Memoria(txtResultado.Text);
        }

        private void btnMC_Click(object sender, EventArgs e)
        {
            try
            {
                lb1Mem = new Label[0];
                panelMemoria.Controls.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMR_Click(object sender, EventArgs e)
        {
            try
            {
                string a = pnMem.Name.Remove(0, 2);
                lbl1Mem = lb1Mem[int.Parse(a) - 1];

                txtOperacion.Text = lbl1Mem.Text;

                varClean = true;
                funcParentesis = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMPlus_Click(object sender, EventArgs e)
        {
            try
            {
                double soma = double.Parse(txtResultado.Text);
                double a = double.Parse(lb1Mem[lb1Mem.Length - 1].Text);
                double resultado = soma + a;
                panelMemoria.Controls.Remove(pnMem);
                Memoria(resultado.ToString());
                varClean = true;
                funcParentesis = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMminus_Click(object sender, EventArgs e)
        {
            try
            {
                double soma = double.Parse(txtResultado.Text);
                double a = double.Parse(lb1Mem[lb1Mem.Length - 1].Text);
                double resultado = a - soma;
                panelMemoria.Controls.Remove(pnMem);
                Memoria(resultado.ToString());
                varClean = true;
                funcParentesis = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion


        #region CALCULOSSINPARSER

        private double CalcularOperacion(string operacion)
        {
            if (operacion.Contains("!"))
            {
                return CalcularFactorial(operacion);
            }
            else if (operacion.Contains("**"))
            {
                return CalcularPotencia(operacion);
            }
            else if (operacion.Contains("%"))
            {
                return CalcularPorcentaje(operacion);
            }
            else if (operacion.StartsWith("1/"))
            {
                return CalcularInverso(operacion);
            }
            else if (operacion.Contains("ln("))
            {
                return CalcularLogaritmoNatural(operacion);
            }
            else if (operacion.Contains("^2"))
            {
                double num = double.Parse(operacion.Replace("^2", ""));
                return Math.Pow(num, 2);
            }
            else
            {
                //parser.Parse(operacion);
                //double resultado = double.Parse(parser.resultado);
                //resultado = AplicarConversionModo(resultado, modo);
                //return resultado;

                try
                {
                    parser.Parse(operacion);
                    double resultado = double.Parse(parser.resultado);
                    resultado = AplicarConversionModo(resultado, modo);
                    return resultado;
                }
                catch (Exception ex)
                {
                    return double.NaN; // Devolver NaN (No es un Número) en caso de error
                }
            }
        }

        private double CalcularFactorial(string operacion)
        {
            double num = double.Parse(operacion.Replace("!", ""));
            double factorial = 1;

            for (double i = num; i > 0; i--)
            {
                factorial *= i;
            }

            return factorial;
        }

        private double CalcularPotencia(string operacion)
        {
            int indiceExponente = operacion.IndexOf("**");
            double baseNum = double.Parse(operacion.Substring(0, indiceExponente));
            double exponente = double.Parse(operacion.Substring(indiceExponente + 2));
            return Math.Pow(baseNum, exponente);
        }

        private double CalcularPorcentaje(string operacion)
        {
            double num = double.Parse(operacion.Replace("%", "")) / 100;
            return num;
        }

        private double CalcularInverso(string operacion)
        {
            double num = double.Parse(operacion.Substring(2));
            return 1 / num;
        }

        private double CalcularLogaritmoNatural(string operacion)
        {
            double num = double.Parse(operacion.Replace("ln(", "").Replace(")", ""));
            return Math.Log(num);
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            //string conta = txtOperacion.Text + txtResultado.Text;
            //double resultado = Evaluate(conta);
            //parser.Parse(txtOperacion.Text);
            //txtResultado.Text = parser.resultado;
            //Historial(resultado.ToString(), conta);

            Font fuenteNormal = new Font("Product Sans", 48, FontStyle.Bold);
            Font fuenteErrorMensaje = new Font("Product Sans", 40, FontStyle.Bold);


            string operacion = txtOperacion.Text;
            double resultado = CalcularOperacion(operacion);

            if (double.IsNaN(resultado))
            {
                txtResultado.Font = fuenteErrorMensaje;
                txtResultado.Text = "Error léxico o sintactico";
            }
            else
            {
                txtResultado.Font = fuenteNormal;
                txtResultado.Text = resultado.ToString();
            }

            Historial(resultado.ToString(), operacion);
        }

        #endregion


        #region MODOS
        private void btnMode_Click(object sender, EventArgs e)
        {
            switch (modo)
            {
                case "deg":
                    btnMode.Text = "RAD";
                    modo = "rad";
                    break;
                case "rad":
                    btnMode.Text = "GRAD";
                    modo = "grad";
                    break;
                case "grad":
                    btnMode.Text = "DEG";
                    modo = "deg";
                    break;
            }
        }

        private double AplicarConversionModo(double valor, string modo)
        {
            switch (modo)
            {
                case "rad":
                    return ConvertirARadianes(valor);
                case "grad":
                    return ConvertirAGradianes(valor);
                default:
                    return valor; // No se aplica conversión en modo RAD
            }
        }

        private double ConvertirARadianes(double angulo)
        {
            return (angulo / 180) * Math.PI;
        }

        private double ConvertirAGradianes(double angulo)
        {
            return (angulo / Math.PI) * 200;
        }


        #endregion
    }
}
