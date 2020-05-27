import {Authority, useInterecptor} from '../../core/ocm';
import * as axios from 'axios';

useInterecptor(axios);

//@ts-ignore
window.auth = new Authority(axios);