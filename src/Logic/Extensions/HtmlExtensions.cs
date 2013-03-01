namespace ScBootstrap.Logic.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using Sitecore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;

    public static class HtmlExtensions
    {
        #region Sitecore fields

        public static HtmlString FieldFor<TModel, TProperty>(this HtmlHelper helper, TModel model, Expression<Func<TModel, TProperty>> expression) where TModel : class
        {
            var type = typeof(TModel);

            var member = expression.Body as MemberExpression;
            if (member == null) throw new ArgumentException(string.Format("Expression '{0}' refers to a method, not a property.", expression));

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null) throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.", expression));

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format("Expresion '{0}' refers to a property that is not from type {1}.", expression, type));

            var fieldName = propInfo.Name;
            return FieldFor(helper, model, fieldName);
        }

        public static HtmlString FieldFor<TModel>(this HtmlHelper helper, TModel model, string fieldName) where TModel : class
        {
            var type = typeof(TModel);

            var propertyId = type.GetProperty("Id");
            if (propertyId == null) throw new ArgumentException("Model must have a property named Id");

            var value = propertyId.GetValue(model, null);
            if (!(value is Guid)) throw new ArgumentException("Id must be type of Guid");

            var id = (Guid) value;
            return FieldFor(helper, id, fieldName);
        }

        public static HtmlString FieldFor(this HtmlHelper helper, Guid id, string fieldName)
        {
            var item = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(id));
            if (item == null) throw new Exception("Item is null");
            return helper.Sitecore().Field(fieldName, item);
        }

        public static HtmlString FieldOrName(this HtmlHelper helper, string fieldName)
        {
            return helper.Sitecore().CurrentItem[fieldName].IsNullOrWhiteSpace()
                ? new HtmlString(helper.Sitecore().CurrentItem.Name)
                : helper.Sitecore().Field(fieldName, new {DisableWebEdit = true});
        }

        #endregion Sitecore fields

        #region Label extensions

        public static HtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return LabelFor(html, expression, new RouteValueDictionary(htmlAttributes));
        }

        public static HtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText)) return new HtmlString(string.Empty);

            var tag = new TagBuilder("label");
            tag.MergeAttributes(htmlAttributes);
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(labelText);
            return new HtmlString(tag.ToString(TagRenderMode.Normal));
        }

        #endregion Label extensions
    }
}
