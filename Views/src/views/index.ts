import {use as Depart} from './LocDepartInfo';
import {use as Duty} from './LocDutyInfo';
import {use as Work} from './LocWorkInfo';
import {use as Layout} from './Layout';

export function use(){
  Depart();
  Duty();
  Work();
  Layout();
}