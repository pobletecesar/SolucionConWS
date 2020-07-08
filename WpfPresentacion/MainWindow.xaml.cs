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
using System.Xml.Serialization;
using BLOG.Entidades;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;
namespace WpfPresentacion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LlenarGrilla();
        }
        private void LlenarGrilla()
        {
            XmlSerializer xmlSerial = new XmlSerializer(typeof(List<Post>));
            ServiceReference1.SvcPostClient svcCliente = new ServiceReference1.SvcPostClient();
            string xmlResp = svcCliente.ListarTodo();

            StringReader xmlRead = new StringReader(xmlResp);

            List<Post> lPost = (List<Post>)xmlSerial.Deserialize(xmlRead);

            dgListPost.ItemsSource = lPost;

        }

        private void ExportarPDF()
        {
            try
            {
                XmlSerializer xmlSerial = new XmlSerializer(typeof(List<Post>));
                ServiceReference1.SvcPostClient svcCliente = new ServiceReference1.SvcPostClient();
                string xmlResp = svcCliente.ListarTodo();

                StringReader xmlRead = new StringReader(xmlResp);

                List<Post> lPost = (List<Post>)xmlSerial.Deserialize(xmlRead);


                ///CREACION DE PAGINA PDF
                var pdfDoc = new Document(PageSize.LETTER, 40f, 40f, 60f, 60f);
                string path = $"C://PDFs//report{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
                PdfWriter.GetInstance(pdfDoc,new FileStream(path,FileMode.OpenOrCreate));
                pdfDoc.Open();

                var columnas = 3;
                var colWidth = new[] { 1f, 2f, 4f };

                var tabla = new PdfPTable(colWidth)
                {
                    WidthPercentage = 100,
                    DefaultCell = {MinimumHeight = 22f},
                    HorizontalAlignment = 0
                };

                var celda = new PdfPCell(new Phrase("REPORTE DE POSTS"))
                {
                    Colspan = columnas,
                    HorizontalAlignment = 1,
                    MinimumHeight = 30f
                };

                tabla.AddCell(celda);

                tabla.AddCell("ID");
                tabla.AddCell("Título");
                tabla.AddCell("Post");

                foreach(Post p in lPost)
                {
                    tabla.AddCell(p.id_post.ToString());
                    tabla.AddCell(p.titulo_post);
                    tabla.AddCell(p.texto_post);
                }

                pdfDoc.Add(tabla);
                pdfDoc.Close();

                MessageBox.Show("Reporte Generado");
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void SendMail()
        {
            try
            {
                MailMessage mail = new MailMessage();

                var SmtpServer = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("", "")
                };

                mail.From = new MailAddress("cesar2kx@gmail.com");
                mail.To.Add("c.pobletes@profesor.duoc.cl");
                mail.To.Add("dieg.mirandam@alumnos.duoc.cl");
                mail.To.Add("joa.silvas@alumnos.duoc.cl");
                mail.Subject = "REPORTE BLOG";
                mail.Body = "Adjuntamos reporte del blog";

                string fileName = cboReporte.SelectedItem.ToString();
                System.Net.Mail.Attachment att;
                att = new System.Net.Mail.Attachment($"C://PDFs//{fileName}");

                mail.Attachments.Add(att);

                SmtpServer.Send(mail);
                lblReporte.Visibility = Visibility.Hidden;
                cboReporte.Visibility = Visibility.Hidden;
                btnMail.Visibility = Visibility.Hidden;
                MessageBox.Show("Mail Enviado");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void BtnGrabar_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerial = new XmlSerializer(typeof(Post));
            ServiceReference1.SvcPostClient svcCliente = new ServiceReference1.SvcPostClient();
            StringWriter xmlWrite = new StringWriter();
            Post oPost = new Post();

            oPost.titulo_post = txtTitulo.Text;
            oPost.texto_post = txtPost.Text;
            xmlSerial.Serialize(xmlWrite, oPost);



            if(svcCliente.Crear(xmlWrite.ToString()))
            {
                MessageBox.Show("Datos Grabados!! XD");
                LlenarGrilla();
            }
            else
            {
                MessageBox.Show("Ups!! Ocurrió un error. No se grabaron los datos");
            }

        }

        private void DgListPost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            Post selected_post = dg.SelectedItem as Post;
            if(selected_post != null)
            {
                txtID.Text = selected_post.id_post.ToString();
                txtTitulo.Text = selected_post.titulo_post;
                txtPost.Text = selected_post.texto_post;
            }
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerial = new XmlSerializer(typeof(Post));
            ServiceReference1.SvcPostClient svcCliente = new ServiceReference1.SvcPostClient();
            StringWriter xmlWrite = new StringWriter();
            Post oPost = new Post();

            oPost.id_post = int.Parse(txtID.Text);
            oPost.titulo_post = txtTitulo.Text;
            oPost.texto_post = txtPost.Text;
            xmlSerial.Serialize(xmlWrite, oPost);



            if (svcCliente.Modificar(xmlWrite.ToString()))
            {
                MessageBox.Show("Datos Modificados!! XD");
                LlenarGrilla();
            }
            else
            {
                MessageBox.Show("Ups!! Ocurrió un error. No se modificaron los datos");
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerial = new XmlSerializer(typeof(Post));
            ServiceReference1.SvcPostClient svcCliente = new ServiceReference1.SvcPostClient();
            StringWriter xmlWrite = new StringWriter();
            Post oPost = new Post();

            oPost.id_post = int.Parse(txtID.Text);

            xmlSerial.Serialize(xmlWrite, oPost);



            if (svcCliente.Eliminar(xmlWrite.ToString()))
            {
                MessageBox.Show("Datos Eliminados!! XD");
                LlenarGrilla();
            }
            else
            {
                MessageBox.Show("Ups!! Ocurrió un error. No se eliminar los datos");
            }
        }

        private void BtnPDF_Click(object sender, RoutedEventArgs e)
        {
            ExportarPDF();
        }

        private void BtnEnviar_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\PDFs");
            FileInfo[] Files = d.GetFiles("*.pdf");

            foreach(FileInfo f in Files)
            {
                cboReporte.Items.Add(f.Name);
            }
            lblReporte.Visibility = Visibility.Visible;
            cboReporte.Visibility = Visibility.Visible;
            btnMail.Visibility = Visibility.Visible;

        }

        private void BtnMail_Click(object sender, RoutedEventArgs e)
        {
            SendMail();
        }
    }
}
