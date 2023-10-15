using QRCoder;
using System.Net.Mail;
using System.Net;

namespace PetPass_API.Services
{
    public class QRCodeService
    {
        public void GenerateAndSendQRCode(int petId, string email)
        {
            string qrCodeText = petId.ToString();
            byte[] qrCodeImageBytes = GenerateQRCodeImage(qrCodeText);

            SendQRCodeByEmail(qrCodeImageBytes, email);
        }

        private byte[] GenerateQRCodeImage(string qrCodeText)
        {
            QRCodeGenerator qrGen = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGen.CreateQrCode(qrCodeText, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

            return qrCode.GetGraphic(20);
        }

        private void SendQRCodeByEmail(byte[] qrCodeImageBytes, string email)
        {
            string EmailOrigin = "nahuel.gutierrez.vargas17@gmail.com";
            string password = "pbek lzxr uxvd byux\r\n";
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;

            MailMessage mailMessage = new MailMessage(EmailOrigin, email, "PETPASS - Código QR de Mascota", "Este es el Código QR UNICO de su Mascota:");
            mailMessage.IsBodyHtml = true;

            // Adjuntar el código QR como un archivo adjunto
            mailMessage.Attachments.Add(new Attachment(new System.IO.MemoryStream(qrCodeImageBytes), "QRCode.png"));

            using (SmtpClient smtpClient = new SmtpClient(smtpServer))
            {
                smtpClient.Port = smtpPort;
                smtpClient.Credentials = new NetworkCredential(EmailOrigin, password);
                smtpClient.EnableSsl = true;

                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    // Manejar errores o registrarlos según tus necesidades
                    Console.WriteLine("Error sending email: " + ex.Message);
                }
            }
        }
    }
}
