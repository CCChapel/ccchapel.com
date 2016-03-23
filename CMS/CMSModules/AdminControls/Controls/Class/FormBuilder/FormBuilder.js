var FormBuilder = (function () {

    var postbackInProgress = false;
    var isFormEmpty = false;
    var saveLabel;
    var savingDelayer;
    var savedDelayer;
    var savedMsgDelayer;
    var allowSavedMsg = false;

    function init() {
        $cmsj(".editing-form-category-fields").sortable({
            cursor: "move",

            // Allow moving fields between categories
            connectWith: ".editing-form-category-fields",

            // A class name that gets applied to placeholder
            placeholder: "sortable-placeholder",

            // Prevents dragging from starting on specified elements
            cancel: ".form-control, .empty-form-placeholder",

            // Move item when mouse pointer overlaps the other item
            tolerance: "pointer",

            over: function () {
                if (isFormEmpty) {
                    $cmsj('.empty-form-placeholder').hide();
                    $cmsj('.empty-form').addClass('Green');
                    $cmsj('.sortable-placeholder').hide();
                }
            },

            out: function () {
                if (isFormEmpty) {
                    $cmsj('.empty-form-placeholder').show();
                    $cmsj('.empty-form').removeClass('Green');
                    $cmsj('.sortable-placeholder').show();
                }
            },

            stop: function () {
                if (isFormEmpty) {
                    $cmsj('.empty-form-placeholder').remove();
                    isFormEmpty = false;
                }
            },

            start: function (event, ui) {
                // This ensure that height of placeholder is equal to dragged control height
                ui.placeholder.height(ui.helper.height());
            },

            update: function (event, ui) {
                var isNewItem = ui.item.hasClass('ui-draggable');
                var index = ui.item.index();
                var category = getCategoryNameFromClassList(ui.item.closest(".editing-form-category").attr('class'));

                if (isNewItem) {
                    var componentName = getComponentFromClassList(ui.item.attr('class'));
                    addNewField(componentName, category, index);
                } else {
                    var fieldName = getFieldNameFromId(ui.item.attr('id'));

                    // Prepare action data
                    var data = "move:" + fieldName + ":" + category + ":" + index;

                    // Send position to server
                    doFieldAction(data);
                }

                showSavingInfo();
            },
        });


        // We need 'change' event before 'mousedown' event and
        // setTimeout with 1 ms is workaround that helps 
        // achieve this behavior.
        $cmsj('.editing-form-category-fields > div').on('mousedown', function () {
            var context = this;
            setTimeout(function () {
                onFieldMouseDown.call(context);
            }, 1);
        });


        // De-selecting field after clicking outside form
        $cmsj('.form-builder-form').on('click', deselectFieldAndHidePanel);
        
        var pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();

        pageRequestManager.add_beginRequest(function (sender, args) {
            postbackInProgress = true;
        });


        pageRequestManager.add_endRequest(function (sender, args) {
            postbackInProgress = false;
            restoreScrollPosition();
        });


        // Save scroll position to be able restore it after postback
        $cmsj('.form-builder-form').on('scroll', function () {
            FormBuilder.scrollPosition = this.scrollTop;
        });


        saveLabel = document.getElementById('lblSaveInfo');
    }


    // Creates new field based on the component name on position specified by category name and index.
    function addNewField(componentName, category, index) {
        var data = "addField:" + componentName + ":" + category + ":" + index;
        doPostBack(data);
        //console.log(data);

        // Reset empty form flag
        isFormEmpty = false;
    }

    function onFieldMouseDown() {
        if (!$cmsj(this).hasClass('selected-field')) {
            highlightSelectedField(this);
            reloadSettingsPanel(this);
        }
    }


    function highlightSelectedField(field) {
        removeClassesFromSelectedFields();
        $cmsj(field).addClass('selected-field');
    }


    function removeClassesFromSelectedFields() {
        $cmsj('.editing-form-category-fields .selected-field').each(function (index, element) {
            $cmsj(element).removeClass('selected-field');
        });
    }


    function reloadSettingsPanel(field) {
        var fieldName = getFieldNameFromId(field.id);
        var data = "loadSettings:" + fieldName;

        // Reload settings
        doPostBack(data);
    }


    function restoreScrollPosition() {
        $cmsj('.form-builder-form').scrollTop(FormBuilder.scrollPosition);
    }


    function deselectFieldAndHidePanel(e) {
        if (e.target === this) {
            if ($cmsj('.editing-form-category-fields .selected-field').length) {
                removeClassesFromSelectedFields();
                hideSettingsPanel();
            }
        }
    }


    function getFieldNameFromId(id) {
        // substr(6) removes 'field_' prefix
        return id.substr(6);
    }


    function getComponentFromClassList(cssClasses) {
        // Regular expression to mach 'component_*' pattern
        var regComponent = /component_\w+/;
        var cssClass = regComponent.exec(cssClasses)[0];

        // substr(10) removes 'component_' prefix
        return cssClass.substr(10);
    }


    function hideSettingsPanel() {
        doPostBack('hideSettingsPanel');
    }


    function selectField(fieldName) {
        var field = document.getElementById("field_" + fieldName);
        if (field) {
            highlightSelectedField(field);
        }
    }


    function setFocusToInput() {
        setTimeout(function () {
            $cmsj('.settings-panel .field-property input').first().focus();
        }, 0);
    }


    function doPostBack(command) {
        if (postbackInProgress) {
            setTimeout(function () {
                doPostBack(command);
            }, 100);
        }
        else {
            __doPostBack("hdnFormBuilderUpdate", command);
        }
    }


    function saveSettings() {
        var value = $cmsj(this).is(':checkbox') ? this.checked : this.value;

        if ($cmsj(this).is(':radio') || this.previousValue !== value) {
            doPostBack('saveSettings');
            this.previousValue = value;

            showSavingInfo();
        }
    }


    function setSaveSettingsTimeout() {
        clearTimeout(this.delayer);

        // context must be set as variable, because setTimeout function 
        // is always run under global context, so in this case we 
        // use variable "context" as pointer to our original context.
        var context = this;
        this.delayer = setTimeout(function () {
            $cmsj(context).trigger('change');
        }, 2000);
    }


    function receiveServerData(arg, context) {
        var data = arg.split(":");

        switch (data[0]) {

            case "error":
                // Display error message
                var errorLabel = $cmsj('.form-builder-error-hidden');
                var closeElem = $cmsj('<span></span>').addClass('close icon-modal-close').click(function () { errorLabel.hide(); });
                errorLabel.addClass("form-builder-error");
                errorLabel.text(data[1].replace("##COLON##", ":"));
                errorLabel.append(closeElem);
                break;
            default:
                showSavedInfo();
                break;
        }
    }


    function showEmptyFormPlaceholder() {
        var emptyForm = $cmsj('.editing-form-category-fields');
        var placeholder = $cmsj('.empty-form-placeholder');

        isFormEmpty = true;
        emptyForm.append(placeholder);
        emptyForm.addClass('empty-form');
        placeholder.show();
    }


    function getCategoryNameFromClassList(cssClasses) {
        // Regular expression to mach 'category_*' pattern
        var regComponent = /category_\w+/;
        var matches = regComponent.exec(cssClasses);

        // substr(9) removes 'category_' prefix
        return (matches) ? matches[0].substr(9) : "";
    }


    function showSavingInfo() {
        clearTimeout(savingDelayer);
        clearTimeout(savedDelayer);
        clearTimeout(savedMsgDelayer);
        allowSavedMsg = false;

        saveLabel.textContent = (typeof msgSaving != 'undefined') ? msgSaving : 'Saving...';
        saveLabel.style.display = '';
        
        // Hide info in case of a long delay (possible error)
        savingDelayer = setTimeout(function () { saveLabel.style.display = 'none'; }, 5000);

        // Make sure the saving message is not overwritten immediately with the saved message
        savedMsgDelayer = setTimeout(function() { allowSavedMsg = true; }, 700);
    }


    function showSavedInfo() {
        if (allowSavedMsg) {
            clearTimeout(savingDelayer);

            saveLabel.textContent = (typeof msgSaved != 'undefined') ? msgSaved : 'All changes saved';
            saveLabel.style.display = '';
        } else {
            savedDelayer = setTimeout(function() { showSavedInfo(); }, 200);
        }
    }


    return {
        receiveServerData: receiveServerData,
        setSaveSettingsTimeout: setSaveSettingsTimeout,
        saveSettings: saveSettings,
        init: init,
        selectField: selectField,
        doPostBack: doPostBack,
        showEmptyFormPlaceholder: showEmptyFormPlaceholder,
        setFocusToInput: setFocusToInput,
        addNewField: addNewField,
        showSavingInfo: showSavingInfo,
        showSavedInfo: showSavedInfo
    };
}());

$cmsj(FormBuilder.init);


$cmsj(function () {
    // Init left panel with draggable components
    $cmsj('.form-components div div').draggable({
        connectToSortable: ".editing-form-category-fields",
        helper: "clone"
    });
});

