import {ViewModel} from './viewModel';
//@ts-ignore
import * as template from './view.html';
import './style.scss';

import {registerCompoent} from '../utils';
import {use as IFrame} from './iframe';

export function use(){
    registerCompoent('ke-layout',{
        viewModel:ViewModel,
        template:template
    });
    IFrame();
}