import { Injectable } from '@angular/core';
import {
    Router,
    Resolve,
    RouterStateSnapshot,
    ActivatedRouteSnapshot
} from '@angular/router';
import { EMPTY, Observable, of } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

import { WorkflowTypeView } from 'app/core/models/workflow-type';
import { WorkflowTypeService } from '../workflow-type/services/workflow-type-service';

@Injectable()
export class WorkflowTypeResolver implements Resolve<WorkflowTypeView> {
    constructor(
        private workflowTypeService: WorkflowTypeService,
        private router: Router
    ) { }

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<WorkflowTypeView> {
        // Param from route
        const id = route.paramMap.get('id') as string;

        return this.workflowTypeService.getWorkflowTypeView(id).pipe(
            switchMap((res: any) => {
                return of(res.data);
            }),
            catchError((err) => {
                if (this.router.url.indexOf('workflow-types/edit') === -1) {
                    this.router.navigateByUrl('/workflow-settings/workflow-types');
                }
                return EMPTY;
            })
        );
    }
}
