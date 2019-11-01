using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraficadorSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            mostrarSegundaSeñal(false);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void btnGraficar_Click(object sender, RoutedEventArgs e)
        {
            /*double amplitud =
                double.Parse(txtAmplitud.Text);
            double fase =
                double.Parse(txtFase.Text);
            double frecuencia =
                double.Parse(txtFrecuencia.Text);*/
            double tiempoInicial =
                double.Parse(txtTiempoInicial.Text);
            double tiempoFinal =
                double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo =
                double.Parse(txtFrecuenciaMuestreo.Text);

            Señal señal;
            Señal segundaSeñal = null;
            Señal señalResultante;

            switch (cbTipoSenal.SelectedIndex)
            {
                case 0: //parabolica
                    señal = new SenalParabolica();
                    break;
                case 1: //senoidal
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtAmplitud.Text);
                    double fase = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtFase.Text);
                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txtFrecuencia.Text);

                    señal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;
                case 2: //exponencial
                    double alfa = double.Parse(((ConfiguracionSeñalExponencial)(panelConfiguracion.Children[0])).txtAlfa.Text);

                    señal = new SeñalExponencial(alfa);
                    break;
                case 3: //audio
                    string rutaArchivo = ((ConfiguracionSeñalAudio)(panelConfiguracion.Children[0])).txtRutaArchivo.Text;
                    señal = new SeñalAudio(rutaArchivo);

                    txtTiempoInicial.Text = señal.TiempoInicial.ToString();
                    txtTiempoFinal.Text = señal.FrecuenciaMuestreo.ToString();

                    break;
                default:
                    señal = null;
                    break;
            }

            if (cbTipoSenal.SelectedIndex != 3 && señal != null)
            {
                señal.TiempoInicial = tiempoInicial;
                señal.TiempoFinal = tiempoFinal;
                señal.FrecuenciaMuestreo = frecuenciaMuestreo;

                señal.construirSeñal();
            }

            //construir segunda señal si es necesario
            if (cbOperacion.SelectedIndex == 2)
            {
                switch (cbTipoSenal_2.SelectedIndex)
                {
                    case 0: //parabolica
                        segundaSeñal = new SenalParabolica();
                        break;
                    case 1: //senoidal
                        double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion_2.Children[0])).txtAmplitud.Text);
                        double fase = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion_2.Children[0])).txtFase.Text);
                        double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion_2.Children[0])).txtFrecuencia.Text);

                        segundaSeñal = new SeñalSenoidal(amplitud, fase, frecuencia);
                        break;
                    case 2: //exponencial
                        double alfa = double.Parse(((ConfiguracionSeñalExponencial)(panelConfiguracion_2.Children[0])).txtAlfa.Text);

                        segundaSeñal = new SeñalExponencial(alfa);
                        break;
                    case 3: //audio
                        string rutaArchivo = ((ConfiguracionSeñalAudio)(panelConfiguracion_2.Children[0])).txtRutaArchivo.Text;
                        segundaSeñal = new SeñalAudio(rutaArchivo);

                        txtTiempoInicial.Text = segundaSeñal.TiempoInicial.ToString();
                        txtTiempoFinal.Text = segundaSeñal.FrecuenciaMuestreo.ToString();

                        break;
                    default:
                        señal = null;
                        break;
                }
                if(cbTipoSenal_2.SelectedIndex != 2 && segundaSeñal != null)
                {
                    segundaSeñal.TiempoInicial = tiempoInicial;
                    segundaSeñal.TiempoFinal = tiempoFinal;
                    segundaSeñal.FrecuenciaMuestreo = frecuenciaMuestreo;

                    segundaSeñal.construirSeñal();
                }
            }

            switch (cbOperacion.SelectedIndex)
            {
                case 0: //escala de amplitud
                    double factorEscala = double.Parse(((OperacionEscalaAmplitud)(panelConfiguracionOperacion.Children[0])).txtFactorEscala.Text);
                    señalResultante = Señal.escalarAmplitud(señal, factorEscala);
                    break;
                case 1: // desplazamiento
                    double desplazamiento = double.Parse(((OperacionDesplazamiento)(panelConfiguracionOperacion.Children[0])).txtDesplazamiento.Text);
                    señalResultante = Señal.desplazamintoSeñales(señal, desplazamiento);
                    break;
                case 2: //multiplicacion de señales
                    señalResultante = Señal.multiplicarSeñales(señal, segundaSeñal);
                    break;
                case 3: //escala exponencial
                    double exponente = double.Parse(((OperacionEscalaExponencial)(panelConfiguracionOperacion.Children[0])).txtExponente.Text);
                    señalResultante = Señal.escalarExponencial(señal, exponente);
                    break;
                case 4: //transformada de fourier
                    señalResultante = Señal.transformadaFourier(señal);
                    break;
                default:
                    señalResultante = null;
                    break;
            }

            // elije entre la primera y la resultante
            double amplitudMaxima = (señal.AmplitudMaxima >= señalResultante.AmplitudMaxima) ?
                señal.AmplitudMaxima : señalResultante.AmplitudMaxima;
            if (segundaSeñal != null)
            {
                //elije entre la mas grande de la primera y resultante y la segunda
                amplitudMaxima = (amplitudMaxima > segundaSeñal.AmplitudMaxima) ?
                    amplitudMaxima : segundaSeñal.AmplitudMaxima;
            }

            double periodoMuestreo = 1.0 / frecuenciaMuestreo;

            plnGrafica.Points.Clear();
            plnGraficaResultante.Points.Clear();
            plnGrafica_2.Points.Clear();

            if (segundaSeñal != null)
            {
                foreach (var muestra in segundaSeñal.Muestras)
                {
                    plnGrafica_2.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
                }
            }
                
            foreach (Muestra muestra in señal.Muestras)
            {
                plnGrafica.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
            }

            foreach(Muestra muestra in señalResultante.Muestras)
            {
                plnGraficaResultante.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
            }

            if (cbOperacion.SelectedIndex == 4)
            {
                int indiceMaximo = 0;
                int indiceInicial = (int)((690.0f * (double)(señalResultante.Muestras.Count)) /
                    señalResultante.FrecuenciaMuestreo);
                int indiceIFinal = (int)((950.0f * (double)(señalResultante.Muestras.Count)) /
                    señalResultante.FrecuenciaMuestreo);
                for (int i = indiceInicial; i < indiceIFinal; i++)
                {
                    if (señalResultante.Muestras[i].Y > señalResultante.Muestras[indiceMaximo].Y)
                    {
                        indiceMaximo = i;
                    }
                }
                double frecuencia = (double)(indiceMaximo * señalResultante.FrecuenciaMuestreo) /
                    (double)señalResultante.Muestras.Count;
                lblHerz.Text = frecuencia.ToString("N") + " Hz";

                int indiceInicial2 = (int)((1200.0f * (double)(señalResultante.Muestras.Count)) /
                    señalResultante.FrecuenciaMuestreo);
                int indiceIFinal2 = (int)((1500.0f * (double)(señalResultante.Muestras.Count)) /
                    señalResultante.FrecuenciaMuestreo);
                int indiceMaximo2 = 0;
                for (int i = indiceInicial2; i < indiceIFinal2; i++)
                {
                    if(señalResultante.Muestras[i].Y > señalResultante.Muestras[indiceMaximo2].Y)
                    {
                        indiceMaximo2 = i;
                    }
                }
                double frecuencia2 = (double)(indiceMaximo2 * señalResultante.FrecuenciaMuestreo) /
                    (double)señalResultante.Muestras.Count;
                lblHerz2.Text = frecuencia2.ToString("N") + " Hz";

                if(frecuencia > 600 && frecuencia < 720)
                {
                    if(frecuencia2 > 1150 && frecuencia2 < 1250)
                    {
                        lblTecla.Text = "1";
                    }
                    if (frecuencia2 > 1250 && frecuencia2 < 1350)
                    {
                        lblTecla.Text = "2";
                    }
                    if (frecuencia2 > 1350 && frecuencia2 < 1500)
                    {
                        lblTecla.Text = "3";
                    }
                }
                if (frecuencia > 730 && frecuencia < 810)
                {
                    if (frecuencia2 > 1150 && frecuencia2 < 1250)
                    {
                        lblTecla.Text = "4";
                    }
                    if (frecuencia2 > 1250 && frecuencia2 < 1350)
                    {
                        lblTecla.Text = "5";
                    }
                    if (frecuencia2 > 1350 && frecuencia2 < 1500)
                    {
                        lblTecla.Text = "6";
                    }
                }
                if (frecuencia > 820 && frecuencia < 890)
                {
                    if (frecuencia2 > 1150 && frecuencia2 < 1250)
                    {
                        lblTecla.Text = "7";
                    }
                    if (frecuencia2 > 1250 && frecuencia2 < 1350)
                    {
                        lblTecla.Text = "8";
                    }
                    if (frecuencia2 > 1350 && frecuencia2 < 1500)
                    {
                        lblTecla.Text = "9";
                    }
                }
                if (frecuencia > 900 && frecuencia < 1000)
                {
                    if (frecuencia2 > 1150 && frecuencia2 < 1250)
                    {
                        lblTecla.Text = "*";
                    }
                    if (frecuencia2 > 1250 && frecuencia2 < 1350)
                    {
                        lblTecla.Text = "0";
                    }
                    if (frecuencia2 > 1350 && frecuencia2 < 1500)
                    {
                        lblTecla.Text = "#";
                    }
                }
            }

            lblLimiteSuperior.Text = amplitudMaxima.ToString("F");
            lblLimiteInferior.Text = "-" + amplitudMaxima.ToString("F");

            lblLimiteInferiorResultado.Text = "-" + amplitudMaxima.ToString("F");
            lblLimiteSuperiorResultado.Text = amplitudMaxima.ToString("F");

            // original
            plnEjeX.Points.Clear();
            plnEjeX.Points.Add(adaptarCoordenadas(tiempoInicial, 0.0, tiempoInicial, amplitudMaxima));
            plnEjeX.Points.Add(adaptarCoordenadas(tiempoFinal, 0.0, tiempoInicial, amplitudMaxima));

            //resultado
            plnEjeXResultante.Points.Clear();
            plnEjeXResultante.Points.Add(adaptarCoordenadas(tiempoInicial, 0.0, tiempoInicial, amplitudMaxima));
            plnEjeXResultante.Points.Add(adaptarCoordenadas(tiempoFinal, 0.0, tiempoInicial, amplitudMaxima));

            plnEjeY.Points.Clear();
            plnEjeY.Points.Add(adaptarCoordenadas(0.0, amplitudMaxima, tiempoInicial, amplitudMaxima));
            plnEjeY.Points.Add(adaptarCoordenadas(0.0, -amplitudMaxima, tiempoInicial, amplitudMaxima));

            //resultado
            plnEjeYResultante.Points.Clear();
            plnEjeYResultante.Points.Add(adaptarCoordenadas(0.0, amplitudMaxima, tiempoInicial, amplitudMaxima));
            plnEjeYResultante.Points.Add(adaptarCoordenadas(0.0, -amplitudMaxima, tiempoInicial, amplitudMaxima));


        }

        public Point adaptarCoordenadas(double x, double y, double tiempoInicial, double amplitudMaxima)
        {
            return new Point((x - tiempoInicial) * scrGrafica.Width, (-1 *
                (y * (((scrGrafica.Height / 2.0) - 25) / amplitudMaxima))) + (scrGrafica.Height / 2.0));
        }

        private void CbTipoSenal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion.Children.Clear();
            switch (cbTipoSenal.SelectedIndex)
            {
                case 0: //exponencial
                    break;
                case 1: //senoidal
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;
                case 2: //exponencial
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalExponencial());
                    break;
                case 3: //audio
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalAudio());
                    break;
                default:
                    break;
            }
        }

        private void CbOperacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracionOperacion.Children.Clear();
            mostrarSegundaSeñal(false);
            switch (cbOperacion.SelectedIndex)
            {
                case 0: //escala
                    panelConfiguracionOperacion.Children.Add(new OperacionEscalaAmplitud());
                    break;
                case 1: //desplazamiento
                    panelConfiguracionOperacion.Children.Add(new OperacionDesplazamiento());
                    break;
                case 2: //multiplicacion
                    mostrarSegundaSeñal(true);
                    break;
                case 3: //escala exponencial
                    panelConfiguracionOperacion.Children.Add(new OperacionEscalaExponencial());
                    break;
                default:
                    break;
            }
        }

        private void CbTipoSenal_2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion_2.Children.Clear();
            switch (cbTipoSenal_2.SelectedIndex)
            {
                case 0: //exponencial
                    break;
                case 1: //senoidal
                    panelConfiguracion_2.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;
                case 2: //exponencial
                    panelConfiguracion_2.Children.Add(new ConfiguracionSeñalExponencial());
                    break;
                case 3: //audio
                    panelConfiguracion_2.Children.Add(new ConfiguracionSeñalAudio());
                    break;
                default:
                    break;
            }
        }

        void mostrarSegundaSeñal(bool mostrar)
        {
            if (mostrar)
            {
                lblTipoSeñal.Visibility = Visibility.Visible;
                cbTipoSenal_2.Visibility = Visibility.Visible;
                panelConfiguracion_2.Visibility = Visibility.Visible;
            }else
            {
                lblTipoSeñal.Visibility = Visibility.Hidden;
                cbTipoSenal_2.Visibility = Visibility.Hidden;
                panelConfiguracion_2.Visibility = Visibility.Hidden;
            }
        }
    }
}