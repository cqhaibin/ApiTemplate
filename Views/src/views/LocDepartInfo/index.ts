import {ViewModel} from './viewModel';
//@ts-ignore
import * as template from './view.html';
import { registerCompoent } from '../utils';

import {use as Detail} from './detail';

export function use(){
    registerCompoent('ke-locdepartinfo',{
        viewModel: ViewModel,
        template: template
    });
    Detail();
}