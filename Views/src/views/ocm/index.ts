// ocm相关页面组件

import {use as Resource} from './resource';
import {use as Role} from './role';
import {use as User} from './user';
import {use as Login} from './login';
import {use as Reload} from './reLoad';

import './style.scss';

export function use(){
    Resource();
    Role();
    User();
    Login();
    Reload();
}