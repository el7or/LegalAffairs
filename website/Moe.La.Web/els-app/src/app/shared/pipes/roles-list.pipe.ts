import { Pipe, PipeTransform } from '@angular/core';
import { Role } from '../../core/models/role';

@Pipe({
    name: 'rolesList'
  })
  
export class RolesListPipe implements PipeTransform{
    transform(roles: Role[], ...args: any[]) {
        return roles.map(r => r.nameAr).join(' - ');
    }
}
