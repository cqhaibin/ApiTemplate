import {ViewModel} from './viewModel';
//@ts-ignore
import * as template from './view.html';
import './style.scss';
import { registerCompoent } from '../../utils';

export function use(){
    registerCompoent('ke-iframe',{
        viewModel:ViewModel,
        template:template
    });   
}