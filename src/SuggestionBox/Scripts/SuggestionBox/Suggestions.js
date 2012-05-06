window.Require("SuggestionBox.Suggestions");

(function(Suggestions, undefined) {
    Suggestions.urlBase = '';
    Suggestions.suggestionDialog = '<div id="dialog" title="Create Suggestion">'
        + '<span><label style="width:250px;" >Title</label>'
        + '<input type="text" style="width:100%;" id="SuggestionTitle" style="display:block;" />'
        + '</span>'
        + '<span style="display:block;">'
        + '<label style="width:110px">Description</label>'
        + '<textarea id="SuggestionDescription" cols="35" rows="10" style="margin:0px; width:100%"/></span></div>';
    Suggestions.commentDialog = '<div id="dialog" title="Create Comment">'
        + '<span style="display:block;">'
        + '<label style="width:110px">Comment</label>'
        + '<textarea id="Comment" cols="35" rows="14" style="margin:0px; width:100%"/></span></div>';
    Suggestions.approveDialog = '<div id="dialog" title="Approve Suggestion">'
        + '<span style="display:block;">'
        + '<label style="width:250px">Approval Comment</label>'
        + '<textarea id="Comment" cols="35" rows="14" style="margin:0px; width:100%"/></span></div>';
    Suggestions.completeDialog = '<div id="dialog" title="Complete Suggestion">'
        + '<span style="display:block;">'
        + '<label style="width:250px">Completion Comment</label>'
        + '<textarea id="Comment" cols="35" rows="14" style="margin:0px; width:100%"/></span></div>';
    Suggestions.denyDialog = '<div id="dialog" title="Deny Suggestion">'
        + '<span style="display:block;">'
        + '<label style="width:250px">Denial Comment</label>'
        + '<textarea id="Comment" cols="35" rows="14" style="margin:0px; width:100%"/></span></div>';
    Suggestions.deleteDialog = '<div id="dialog" title="Delete Suggestion">'
        + '<span style="display:block;">'
        + '<label style="width:250px">Deletion Comment</label>'
        + '<textarea id="Comment" cols="35" rows="14" style="margin:0px; width:100%"/></span></div>';
    Suggestions.suggestionId = 0;

    Suggestions.deleteComment = function(commentId) {
        if (confirm('Are you sure you want to delete this comment?')) {
            var model = { Id: commentId };
            $.blockUI();
            $.ajax({
                url: Suggestions.urlBase + 'Comments/Delete',
                data: model,
                type: 'POST',
                success: function(data) {
                    $('#commentList').html(data);
                    $.unblockUI();
                }
            });
        }
    };

    Suggestions.setUpListPage = function() {
        $(document).ajaxError(function(event, request, settings) {
            var msg = '<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>Error requesting page ' + settings.url + '</div>';
            $('#msg').append(msg);
            $.unblockUI();
        });
        
        $(document).on('click', '#createSuggestion', null, function() {
            $(Suggestions.suggestionDialog).dialog({
                autoOpen: true,
                width: 800,
                height: 400,
                modal: true,
                resizable: true,
                open: function() { $(this).find("#SuggestionTitle").focus(); },
                buttons: [{
                    text: "Submit",
                    click: function() {
                        var model = { Title: $(this).find("#SuggestionTitle").val(), Body: $(this).find("#SuggestionDescription").val() };

                        $.blockUI();

                        $.ajax({
                            url: Suggestions.urlBase + 'Suggestions/Add',
                            data: model,
                            type: 'POST',
                            success: function(data) {
                                $('#suggestionList').html(data);
                                $.unblockUI();
                            }
                        });

                        $(this).dialog('destroy');
                    }
                }]
            });
        });
    };

    Suggestions.setUpViewPage = function() {
        $(document).ajaxError(function(event, request, settings) {
            var msg = '<div class="alert alert-error"><a class="close" data-dismiss="alert">&times;</a>Error requesting page ' + settings.url + '</div>';
            $('#msg').append(msg);
            $.unblockUI();
        });

        $(document).on('click', '#create', null, function() {
            $(Suggestions.commentDialog).dialog({
                autoOpen: true,
                width: 800,
                height: 400,
                modal: true,
                resizable: true,
                open: function() { $(this).find("#Comment").focus(); },
                buttons: [{
                    text: "Submit",
                    click: function() {
                        var model = { Body: $(this).find("#Comment").val(), SuggestionId: SuggestionBox.Suggestions.suggestionId };
                        $.blockUI();
                        $.ajax({
                            url: SuggestionBox.Suggestions.urlBase + 'Comments/Add',
                            data: model,
                            type: 'POST',
                            success: function(data) {
                                $('#commentList').html(data);
                                $.unblockUI();
                            }
                        });

                        $(this).dialog('destroy');
                    }
                }]
            });
        });

        $(document).on('click', '#approve', null, function() {
            $(Suggestions.approveDialog).dialog({
                autoOpen: true,
                width: 800,
                height: 400,
                modal: true,
                resizable: true,
                open: function() { $(this).find("#Comment").focus(); },
                buttons: [{
                    text: "Submit",
                    click: function() {
                        var model = { commentText: $(this).find("#Comment").val(), Id: SuggestionBox.Suggestions.suggestionId };
                        $.blockUI();
                        $.ajax({
                            url: SuggestionBox.Suggestions.urlBase + 'Suggestions/Approve',
                            data: model,
                            type: 'POST',
                            success: function(data) {
                                $('#suggestion').html(data);
                                $.unblockUI();
                            }
                        });

                        $(this).dialog('destroy');
                    }
                }]
            });
        });

        $(document).on('click', '#complete', null, function() {
            $(Suggestions.completeDialog).dialog({
                autoOpen: true,
                width: 800,
                height: 400,
                modal: true,
                resizable: true,
                open: function() { $(this).find("#Comment").focus(); },
                buttons: [{
                    text: "Submit",
                    click: function() {
                        var model = { commentText: $(this).find("#Comment").val(), Id: SuggestionBox.Suggestions.suggestionId };
                        $.blockUI();
                        $.ajax({
                            url: SuggestionBox.Suggestions.urlBase + 'Suggestions/Complete',
                            data: model,
                            type: 'POST',
                            success: function(data) {
                                $('#suggestion').html(data);
                                $.unblockUI();
                            }
                        });

                        $(this).dialog('destroy');
                    }
                }]
            });
        });

        $(document).on('click', '#deny', null, function() {
            $(Suggestions.denyDialog).dialog({
                autoOpen: true,
                width: 800,
                height: 400,
                modal: true,
                resizable: true,
                open: function() { $(this).find("#Comment").focus(); },
                buttons: [{
                    text: "Submit",
                    click: function() {
                        var model = { commentText: $(this).find("#Comment").val(), Id: SuggestionBox.Suggestions.suggestionId };
                        $.blockUI();
                        $.ajax({
                            url: SuggestionBox.Suggestions.urlBase + 'Suggestions/Deny',
                            data: model,
                            type: 'POST',
                            success: function(data) {
                                $('#suggestion').html(data);
                                $.unblockUI();
                            }
                        });

                        $(this).dialog('destroy');
                    }
                }]
            });
        });

        $(document).on('click', '#delete', null, function() {
            $(Suggestions.deleteDialog).dialog({
                autoOpen: true,
                width: 800,
                height: 400,
                modal: true,
                resizable: true,
                open: function() { $(this).find("#Comment").focus(); },
                buttons: [{
                    text: "Submit",
                    click: function() {
                        var model = { commentText: $(this).find("#Comment").val(), Id: Suggestions.suggestionId };
                        $.blockUI();
                        $.ajax({
                            url: Suggestions.urlBase + 'Suggestions/Delete',
                            data: model,
                            type: 'POST',
                            success: function(data) {
                                $('#suggestion').html(data);
                                $.unblockUI();
                            }
                        });

                        $(this).dialog('destroy');
                    }
                }]
            });
        });
    };
    
    Suggestions.undeleteComment = function(commentId) {
        if (confirm('Are you sure you want to undelete this comment?')) {
            var model = { Id: commentId };
            $.blockUI();
            $.ajax({
                url: Suggestions.urlBase + 'Comments/Undelete',
                data: model,
                type: 'POST',
                success: function(data) {
                    $('#commentList').html(data);
                    $.unblockUI();
                }
            });
        }
    };
}(window.SuggestionBox.Suggestions = window.SuggestionBox.Suggestions || { }));