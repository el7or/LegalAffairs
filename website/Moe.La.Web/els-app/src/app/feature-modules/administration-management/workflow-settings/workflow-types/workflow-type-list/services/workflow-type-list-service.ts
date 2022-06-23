import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Constants } from 'app/core/constants';
import { DataService } from 'app/core/services/data.service';

@Injectable({ providedIn: 'root' })
export class WorkflowTypeListService extends DataService {
    constructor(http: HttpClient) {
        super(Constants.WORKFLOW_LOOKUPS_API_URL, http);
    }

    /**
     * Get all workflow types.
     */
    GetAllWorkflowTypes() {
        return this.http.get(this.url + "/workflow-types");
    }
}
