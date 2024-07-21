using AngleSharp.Dom;
using AngleSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace M3U8_Downloader
{
    public class M3U8Extractor
    {
        public M3U8Extractor() 
        {

        }

        public async void GetM3U8()
        {
            // HTMLを取得するURL
            string url = ""; // ここに解析したいHTMLのURLを指定してください

            // AngleSharpの設定を構成
            var config = Configuration.Default
                                       .WithDefaultLoader()
                                       .WithJs(); // JavaScriptエンジンを追加
            var context = BrowsingContext.New(config);

            // HTMLを取得して解析
            var document = await context.OpenAsync(url);
            Debug.WriteLine(document.Source.Text);
            // m3u8ファイルのURLを格納するリスト
            List<string> m3u8Links = new List<string>();

            // HTML内のすべての要素をしらみ潰しに探すメソッド
            void FindM3u8Urls(INode node)
            {
                if (node is IElement element)
                {
                    // href属性を持つ要素をチェック
                    var href = element.GetAttribute("href");
                    if (href != null && href.EndsWith(".m3u8"))
                    {
                        m3u8Links.Add(href);
                    }

                    // src属性を持つ要素をチェック
                    var src = element.GetAttribute("src");
                    if (src != null && src.EndsWith(".m3u8"))
                    {
                        m3u8Links.Add(src);
                    }
                }

                // 子要素を再帰的にチェック
                foreach (var child in node.ChildNodes)
                {
                    FindM3u8Urls(child);
                }
            }

            // ドキュメントのルート要素から探索を開始
            FindM3u8Urls(document.DocumentElement);

            // 結果を出力
            foreach (var link in m3u8Links)
            {
                Debug.WriteLine(link);
            }
        }

    }
}
