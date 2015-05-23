/* GitHub(Source): https://GitHub.com/ArachisH

    .NET library for creating Habbo Hotel desktop applications.
    Copyright (C) 2015  Arachis

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/

using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sulakore.Habbo.Web;

namespace Sulakore
{
    /// <summary>
    /// Provides static methods for extracting public information from a specific hotel.
    /// </summary>
    public static class SKore
    {
        private const string USER_API_SUFFIX = "/api/public/users?name=";
        private const string PROFILE_API_FORMAT = "{0}/api/public/users/{1}/profile";
        private const string IP_COOKIE_PREFIX = "YPF8827340282Jdskjhfiw_928937459182JAX666=";

        private static string _ipCookie;

        private static readonly HttpClient _httpClient;
        private static readonly HttpClientHandler _httpClientHandler;
        private static readonly IDictionary<HHotel, IDictionary<string, string>> _uniqueIds;

        static SKore()
        {
            _httpClientHandler = new HttpClientHandler() { UseProxy = false };
            _httpClient = new HttpClient(_httpClientHandler, true);

            _uniqueIds = new Dictionary<HHotel, IDictionary<string, string>>();
        }

        /// <summary>
        /// Returns your external Internet Protocol (IP) address that is required to successfully send GET/POST request to the specified <seealso cref="HHotel"/> in an asynchronous operation.
        /// </summary>
        /// <param name="hotel">The hotel you wish to retrieve the cookie containing your external Internet Protocol (IP) address from.</param>
        /// <returns></returns>
        public static async Task<string> GetIPCookieAsync(HHotel hotel)
        {
            if (!string.IsNullOrEmpty(_ipCookie)) return _ipCookie;
            string body = await _httpClient.GetStringAsync(hotel.ToUrl());

            return _ipCookie = (body.Contains("setCookie")) ?
                IP_COOKIE_PREFIX + body.GetChilds("setCookie", '\'', false)[3] : string.Empty;
        }
        /// <summary>
        /// Returns the <seealso cref="HUser"/> from the specified hotel assoicted with the given name.
        /// </summary>
        /// <param name="name">The name of the player you wish to retrieve the <seealso cref="HUser"/> from.</param>
        /// <param name="hotel">The <seealso cref="HHotel"/> that the target user is located on.</param>
        /// <returns></returns>
        public static async Task<HUser> GetUserAsync(string name, HHotel hotel)
        {
            string userJson = await _httpClient.GetStringAsync(
                hotel.ToUrl() + USER_API_SUFFIX + name);

            return HUser.Create(userJson);
        }

        /// <summary>
        /// Returns the unique identifier from the specified <seealso cref="HHotel"/> associated with the given name.
        /// </summary>
        /// <param name="name">The name of the player you wish to retrieve the unique identifier from.</param>
        /// <param name="hotel">The <seealso cref="HHotel"/> that the target user is located on.</param>
        /// <returns></returns>
        public static async Task<string> GetUniqueIdAsync(string name, HHotel hotel)
        {
            bool hotelInitialized = _uniqueIds.ContainsKey(hotel);

            if (!hotelInitialized)
                _uniqueIds.Add(hotel, new Dictionary<string, string>());
            else if (_uniqueIds[hotel].ContainsKey(name))
                return _uniqueIds[hotel][name];

            string uniqueId = (await GetUserAsync(name, hotel)).UniqueId;

            _uniqueIds[hotel][name] = uniqueId;
            return uniqueId;
        }
        /// <summary>
        /// Returns the <seealso cref="HProfile"/> from the specified hotel assoicted with the given name.
        /// </summary>
        /// <param name="uniqueId">The unique identifier of the player you wish to retrieve the <see cref="HProfile"/> from.</param>
        /// <param name="hotel">The <seealso cref="HHotel"/> that the target user is located on.</param>
        /// <returns></returns>
        public static async Task<HProfile> GetProfileAsync(string uniqueId, HHotel hotel)
        {
            string profileJson = await _httpClient.GetStringAsync(
                string.Format(PROFILE_API_FORMAT, hotel.ToUrl(), uniqueId));

            return HProfile.Create(profileJson);
        }

        /// <summary>
        /// Returns the full url representation of the specified <seealso cref="HHotel"/>.
        /// </summary>
        /// <param name="hotel">The <seealso cref="HHotel"/> you wish to retrieve the full url from.</param>
        /// <returns></returns>
        public static string ToUrl(this HHotel hotel)
        {
            const string HotelUrlFormat = "https://www.Habbo.";
            return HotelUrlFormat + hotel.ToDomain();
        }
        /// <summary>
        /// Returns the domain of the associated <see cref="HHotel"/>.
        /// </summary>
        /// <param name="hotel">The <see cref="HHotel"/> that is associated with the wanted domain.</param>
        /// <returns></returns>
        public static string ToDomain(this HHotel hotel)
        {
            string value = hotel.ToString().ToLower();
            return value.Length != 5 ? value : value.Insert(3, ".");
        }

        /// <summary>
        /// Returns a new string that begins from where the parent ended in the source.
        /// </summary>
        /// <param name="source">The string that is to be processed.</param>
        /// <param name="parent">The string that determines where the substring operation will take place.</param>
        /// <returns></returns>
        public static string GetChild(this string source, string parent)
        {
            return source.Substring(source.IndexOf(parent, StringComparison.OrdinalIgnoreCase) + parent.Length).Trim();
        }
        /// <summary>
        /// Returns a new string that is in between the parent and the delimiter in the source.
        /// </summary>
        /// <param name="source">The string that is to be processed.</param>
        /// <param name="parent">The string that determines where the substring operation will take place.</param>
        /// <param name="delimiter">The Unicode character that will be used to delimit the substring.</param>
        /// <returns></returns>
        public static string GetChild(this string source, string parent, char delimiter)
        {
            return GetChilds(source, parent, delimiter, false)[0].Trim();
        }
        /// <summary>Returns a string array that contains the substrings in the source that are delimited after the parent.
        /// </summary>
        /// <param name="source">The string that is to be processed.</param>
        /// <param name="parent">The string that determines where the substring operation will take place.</param>
        /// <param name="delimiter">The Unicode character that will be used to delimit the substring.</param>
        /// <param name="withNested">True if you wish to... actually, I forgot why I did this, you'll be fine with the default setting.<para>hhhhh</para></param>
        /// <returns></returns>
        public static string[] GetChilds(this string source, string parent, char delimiter, bool withNested = true)
        {
            char[] delis = withNested ? (parent + delimiter).ToCharArray() : new[] { delimiter };
            return GetChild(source, parent).Split(delis, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}