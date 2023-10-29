using Firebase.Auth;
using Firebase.Storage;
using PetPass_API.Models.Custom;

namespace PetPass_API.Services
{
    public class PhotoPetService
    {
        private static readonly string Apikey = "AIzaSyAy7G47flOMO4UxxkygusUw5S1EYrY13Gg";
        private static readonly string Bucket = "fir-petpassdotnet.appspot.com";
        private static readonly string AuthEmail = "petpassdotnet2023@gmail.com";
        private static readonly string AuthPassword = "Petp@ss123*";


        public async Task<List<string>> SubirImagenesMascota(List<string> images, string petName)
        {
            var imageUrls = new List<string>();
            int imageCounter = 1;

            foreach (var base64Image in images)
            {
                ImageDTO imageDTO = new ImageDTO
                {
                    Image = base64Image,
                    ImageName = petName + imageCounter, 
                    FolderPetsName = "Pets",
                    FolderName = petName
                };

                string imageFromFirebase = await Agarratodo(imageDTO);
                imageUrls.Add(imageFromFirebase);
                imageCounter++;
            }

            return imageUrls;
        }

        private static async Task<string> Agarratodo(ImageDTO model)
        {
            var imageFromBase64ToStream = ConvertB64ToStream(model.Image);
            var imageStream = imageFromBase64ToStream.ReadAsStream();
            string imageFromFirebase = await UploadImageBrigadier(imageStream, model);
            return imageFromFirebase;
        }

        private static StreamContent ConvertB64ToStream(string imageFromRequest)
        {
            byte[] imageStringToBase64 = Convert.FromBase64String(imageFromRequest);
            StreamContent streamContent = new(new MemoryStream(imageStringToBase64));
            return streamContent;
        }

        private static async Task<string> UploadImageBrigadier(Stream stream, ImageDTO imageDTO)
        {
            string imageFromFirebaseStorage = "";
            FirebaseAuthProvider firebaseConfiguration = new(new FirebaseConfig(Apikey));

            FirebaseAuthLink authConfiguration = await firebaseConfiguration
                .SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            CancellationTokenSource cancellationToken = new();

            FirebaseStorageTask storageManager = new FirebaseStorage(Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(authConfiguration.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child(imageDTO.FolderPetsName)
                .Child(imageDTO.FolderName)
                .Child(imageDTO.ImageName)
                .PutAsync(stream, cancellationToken.Token);

            try
            {
                imageFromFirebaseStorage = await storageManager;
            }
            catch
            {
            }
            return imageFromFirebaseStorage;
        }
    }
}
