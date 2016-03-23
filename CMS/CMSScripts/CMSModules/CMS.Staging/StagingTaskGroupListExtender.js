cmsdefine(['CMS/EventHub'], function (EventHub, $) {
    'use strict';

    var Module = function (serverData) {
        var parameters = {
            taskGroupDeletedID: serverData.taskGroupDeletedID
        };

        // Publishes event for module StagingTaskGroupMenu.js, that work label was deleted from UniGrid
        EventHub.publish('TaskGroupDeleted', parameters);

    };
    return Module;
});