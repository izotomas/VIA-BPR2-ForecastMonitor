interface ISerie {
  name: string;
  series: {
    name: Date;
    value: number;
  }[];
}

export interface IPlotResponse {
  unit_key: string;
  name?: string;
  historical: {
    x?: string;
    y?: number;
  }[];
  predictions: {
    x?: string;
    y?: number;
  }[];
}

export interface IPlot {
  unit_key: string;
  unit_name?: string;
  data: ISerie[];
  hasData?: boolean;
}
