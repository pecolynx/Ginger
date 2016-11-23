using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger.Models
{
    [TestFixture]
    class PaginationTest
    {
        private readonly bool t = true;
        private readonly bool f = false;

        [Test]
        public void 総件数が0件のとき非表示となること()
        {
            var pagination = new Pagination(0, 0);
            Assert.That(pagination.Visibility, Is.False);
        }

        [Test]
        public void 総件数が1件のとき表示となること()
        {
            var pagination = new Pagination(1, 10);
            Assert.That(pagination.Visibility, Is.True);
        }

        [Test]
        public void 総件数が10件でページあたり5件のときページ数は2となること()
        {
            var pagination = new Pagination(10, 5);
            Assert.That(pagination.LastPage, Is.EqualTo(2));
        }

        [Test]
        public void 総件数が11件でページあたり5件のときページ数は2となること()
        {
            var pagination = new Pagination(11, 5);
            Assert.That(pagination.LastPage, Is.EqualTo(3));
        }

        [Test]
        public void 総件数が11件でページあたり5件で現在ページが1のとき()
        {
            var pagination = new Pagination(11, 5);
            var pageList = pagination.GetPageList(1);

            Assert.That(pageList.Count, Is.EqualTo(2 + 0 + 1 + 2 + 2));

            var pageTextList = pageList.Select(x => x.Text);
            Assert.That(pageTextList, Is.EqualTo(new List<string>() { "<<", "<", "1", "2", "3", ">", ">>" }));

            var pageNumberList = pageList.Select(x => x.PageNumber);
            Assert.That(pageNumberList, Is.EqualTo(new List<int>() { 1, 1, 1, 2, 3, 2, 3 }));

            var enabledList = pageList.Select(x => x.Enabled);
            Assert.That(enabledList, Is.EqualTo(new List<bool>() { f, f, f, t, t, t, t }));
        }

        [Test]
        public void 総件数が11件でページあたり5件で現在ページが2のとき()
        {
            var pagination = new Pagination(11, 5);
            var pageList = pagination.GetPageList(2);

            Assert.That(pageList.Count, Is.EqualTo(2 + 0 + 1 + 2 + 2));

            var pageTextList = pageList.Select(x => x.Text);
            Assert.That(pageTextList, Is.EqualTo(new List<string>() { "<<", "<", "1", "2", "3", ">", ">>" }));


            var pageNumberList = pageList.Select(x => x.PageNumber);
            Assert.That(pageNumberList, Is.EqualTo(new List<int>() { 1, 1, 1, 2, 3, 3, 3 }));

            var enabledList = pageList.Select(x => x.Enabled);
            Assert.That(enabledList, Is.EqualTo(new List<bool>() { t, t, t, f, t, t, t }));
        }

        [Test]
        public void 総件数が50件でページあたり5件で現在ページが2のとき()
        {
            var pagination = new Pagination(50, 5);
            var pageList = pagination.GetPageList(2);

            Assert.That(pageList.Count, Is.EqualTo(2 + 1 + 1 + 3 + 2));

            var pageTextList = pageList.Select(x => x.Text);
            Assert.That(pageTextList, Is.EqualTo(new List<string>() { "<<", "<", "1", "2", "3", "4", "5", ">", ">>" }));


            var pageNumberList = pageList.Select(x => x.PageNumber);
            Assert.That(pageNumberList, Is.EqualTo(new List<int>() { 1, 1, 1, 2, 3, 4, 5, 3, 10 }));

            var enabledList = pageList.Select(x => x.Enabled);
            Assert.That(enabledList, Is.EqualTo(new List<bool>() { t, t, t, f, t, t, t, t, t }));
        }
    }
}
