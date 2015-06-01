using System;

namespace CrescentIsland.Website.Helpers
{
    public static class AvatarHelper
    {
        // Send in User.AvatarImage and User.AvatarMimeType
        public static string GetAvatarUrl(byte[] avatar, string mimetype)
        {
            if (avatar == null) return String.Empty;
            return String.Format("data:image/{0};base64,{1}", mimetype, Convert.ToBase64String(avatar, 0, avatar.Length));
        }
    }
}