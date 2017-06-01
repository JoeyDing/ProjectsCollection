function schedulecontrol_edit(manager, senderUniqueID, type) {
    if (manager && senderUniqueID && type) {
        var ajaxManager = $find(manager);
        var args = "ajaxupdate," + type + "," + senderUniqueID;
        ajaxManager.ajaxRequest(args);
    }
}

if (typeof jQuery != 'undefined') {
    $(document).ready(function () {
        Sys.Application.add_load(function () {
            $('.rgtTask').tooltip(
           {
               title: 'testtitle',
               placement: 'right',
               viewport: { selector: 'body', padding: 0 }
           });
            //rgtTaskTemplate

            $('[data-toggle="tooltip"]').tooltip();
        });
    });
}

//Sys.Application.add_load(function () {
//    $(document).ready(function () {
//        $('.rgtTask').tooltip(
//       {
//           title: 'testtitle',
//           placement: 'right',
//           viewport: { selector: 'body', padding: 0 }
//       });
//        rgtTaskTemplate

//        $('[data-toggle="tooltip"]').tooltip();
//    });
//});

function milestoneDateSelected(fromPickerID, endPickerID) {
    var fromDatePicker = $find(fromPickerID);
    var enDatePicker = $find(endPickerID);
    if (!enDatePicker.get_selectedDate()) {
        var date = fromDatePicker.get_selectedDate();
        enDatePicker.set_selectedDate(date);
    }
}

function milestoneWindowAddMilestoneButton(radPanelBarID, args) {
    var panelBar = $find(radPanelBarID);
    panelBar.trackChanges();

    var newItem = new Telerik.Web.UI.RadPanelItem();
    newItem.set_text("test");
    panelBar.get_items().add(newItem);
    newItem.expand();
    panelBar.commitChanges();
}

function onMilestoneCategoryAddNewReleaseWindowSelectionChanged(comboboxId, radTextBoxId, radTextBoxReleaseNameId) {
    var categoryComboboxOntheTop = $find(comboboxId);
    selectedItem = categoryComboboxOntheTop.get_selectedItem().get_text();
    var milestoneNameOnTheTop = $find(radTextBoxId);
    var milestoneCategories = ["locready", "locstart", "progressing", "endgame", "signoff", "retro"];
    var releaseNameFromAddReleaseWindow = $find(radTextBoxReleaseNameId);
    var releaseName = releaseNameFromAddReleaseWindow.get_value();
    var milesteonName = releaseName + " " + selectedItem;
    //milestoneNameOnTheTop.enable();
    milestoneNameOnTheTop._textBoxElement.readOnly = false;
    milestoneNameOnTheTop.set_value("");

    for (i = 0; i < milestoneCategories.length; i++) {
        if (milestoneCategories[i] == selectedItem) {
            //populate textbox with data
            milestoneNameOnTheTop.set_value(milesteonName);
            //milestoneNameOnTheTop.disable();
            milestoneNameOnTheTop._textBoxElement.readOnly = true;
            break;
        }
    }
}

function onMilestoneCategoryAddNewMilestoneWindowSelectionChanged(comboboxId, radTextBoxId, labelInfo) {
    var categoryComboboxOntheTop = $find(comboboxId);
    selectedItem = categoryComboboxOntheTop.get_selectedItem().get_text();
    var milestoneNameOnTheTop = $find(radTextBoxId);
    var milestoneCategories = ["locready", "locstart", "progressing", "endgame", "signoff", "retro"];
    var releaseName = labelInfo;
    var milesteonName = releaseName + " " + selectedItem;
    //milestoneNameOnTheTop.enable();
    milestoneNameOnTheTop._textBoxElement.readOnly = false;
    milestoneNameOnTheTop.set_value("");

    for (i = 0; i < milestoneCategories.length; i++) {
        if (milestoneCategories[i] == selectedItem) {
            //populate textbox with data
            milestoneNameOnTheTop.set_value(milesteonName);
            //milestoneNameOnTheTop.disable();
            milestoneNameOnTheTop._textBoxElement.readOnly = true;
            break;
        }
    }
}

function onDateChange(fromDatePickerId, toDatePickerId) {
   
    var fromDatePicker = $find(fromDatePickerId);
    var toDatePicker = $find(toDatePickerId);
    var selectedDate = fromDatePicker.get_dateInput().get_selectedDate();
    toDatePicker.set_selectedDate(selectedDate);
}
function categoryNameChanged(comboboxId, radTextBoxId, radTextBoxReleaseName) {
    var categoryComboboxOntheTop = $find(comboboxId);
    selectedItem = categoryComboboxOntheTop.get_selectedItem().get_text();
    var radTextBoxReleaseNameOnTheTop = $find(radTextBoxReleaseName);
    var milestoneNameOnTheTop = $find(radTextBoxId);
    var milestoneCategories = ["locready", "locstart", "progressing", "endgame", "signoff", "retro"];
    var milestoneName = selectedItem;
    var releaseName = radTextBoxReleaseNameOnTheTop.get_textBoxValue() + " ";
    milestoneNameOnTheTop._textBoxElement.readOnly = false;
    milestoneNameOnTheTop.set_value("");

    for (i = 0; i < milestoneCategories.length; i++) {
        if (milestoneCategories[i] == selectedItem) {
            milestoneNameOnTheTop.set_value(releaseName + milestoneName);
            milestoneNameOnTheTop._textBoxElement.readOnly = true;
            break;
        }
    }
}