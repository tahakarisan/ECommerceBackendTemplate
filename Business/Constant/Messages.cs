namespace Business.Constants
{
    public static class Messages
    {
        #region General
        public const string Listed = "Listelendi.";
        public const string NotListed = "Listelenemedi.";
        public const string Added = "Eklendi.";
        public const string NotAdded = "Eklenemedi.";
        public const string Updated = "Güncellendi.";
        public const string NotUpdated = "Güncellenemedi.";
        public const string Deleted = "Silindi.";
        public const string NotDeleted = "Silinemedi.";
        public const string NotFound = "Bulunamadı.";
        public const string Error = "Bir hata oluştu.";
        public const string Found = "Bulundu.";
        #endregion
        #region Category
        public const string ParentCategoryNotFound = "Bağlı Kategori bulunamadı.";
        #endregion
        #region Auth
        public const string AuthorizationDenied = "Erişim Reddedildi";
        public const string UserRegistered = "Kullanıcı Kayıt Oldu.";
        public const string UserNotRegistered = "Kullanıcı Kayıt Edilemedi!";
        public const string UserNotFound = "Kullanıcı Bulunamadı.";
        public const string PasswordError = "Parola Hatalı.";
        public const string SuccessfulLogin = "Başarıyla giriş yapıldı.";
        public const string UserAlreadyExists = "Kullanıcı zaten mecvut.";
        public const string AccessTokenCreated = "Acces Token oluşturuldu.";
        #endregion
        #region Shopping
        public const string AddToBasket = "Add to basket";
        public const string RemoveToBasket = "Remove to basket";
        #endregion
    }
}