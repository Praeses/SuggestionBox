window.Require = function (namespace) {
    ///<summary>
    /// Builds a namespace
    ///</summary>    
    var components = namespace.split(".");
    var parent = window;

    $(components).each(function (i, o) {
        parent[o] = parent[o] || {};
        parent = parent[o];
    });
};

window.SuggestionBox = window.SuggestionBox || {};
