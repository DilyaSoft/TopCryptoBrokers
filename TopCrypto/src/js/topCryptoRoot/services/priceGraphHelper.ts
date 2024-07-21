
class PriceGraphHelper {
  public writeSimpleGraphForObject(tag: string, obj: Object) {
    if (!obj) return;

    var keys = Object.keys(obj);
    for (var i = 0; i < keys.length; i++) {
      var elem = document.getElementById(`${tag}${keys[i]}`);
      this.drawGraphic(<HTMLCanvasElement>elem, obj[keys[i]], 142, 45, null, null);
    }
  }

  public drawGraphic(element: HTMLCanvasElement
    , listOfY: Array<number>
    , width: number
    , height: number
    , labels
    , leftLabels
    , fillStyle = "#000000") {

    if (!element) return;

    var ctx = element.getContext("2d");
    ctx.clearRect(0, 0, element.width, element.height);

    if (!listOfY || !listOfY.length) return;
    if (width <= 0 || height <= 0) return;

    var offsetTop = 0;
    if (labels) {
      height = height - 40;
      offsetTop = 10;
    }

    var offsetLeft = 0;
    if (leftLabels) {
      width -= 50;
      offsetLeft = 50;
    }

    var maxElement = listOfY[0],
      minElement = listOfY[0];
    listOfY.forEach((item) => {
      maxElement = Math.max(maxElement, item);
      minElement = Math.min(minElement, item);
    });

    var cuttedListOfY = listOfY.map(function (val) {
      return val - minElement;
    });

    var zipMultiply = (height / ((maxElement - minElement) * 1.01));

    var dotNum = 0;
    var x1 = 0;
    var x2 = 0;
    var yLast = 0;
    var yPrev = height + offsetTop - cuttedListOfY[0] * zipMultiply;
    var dotCount = cuttedListOfY.length;

    ctx.lineWidth = 3;

    while (dotNum < dotCount - 1) {
      x1 = width * dotNum / (dotCount - 1) + offsetLeft;
      x2 = width * (dotNum + 1) / (dotCount - 1) + offsetLeft;
      yLast = height + offsetTop - cuttedListOfY[dotNum + 1] * zipMultiply;

      ctx.beginPath();
      ctx.moveTo(x1, yPrev);
      ctx.lineTo(x2, yLast);

      if (cuttedListOfY[dotNum + 1] * zipMultiply > height / 3 * 2) {
        ctx.strokeStyle = '#33f0dc';
      }
      else if (cuttedListOfY[dotNum + 1] * zipMultiply > height / 3 * 1) {
        ctx.strokeStyle = '#dcee95';
      }
      else {
        ctx.strokeStyle = '#ffef93';
      }

      ctx.stroke();

      yPrev = yLast;
      dotNum++;
    }

    if (labels && labels.length) {
      ctx.fillStyle = fillStyle;
      ctx.strokeStyle = fillStyle;
      ctx.font = "bold 10pt Arial";

      var labelsLength = labels.length;
      while (dotCount % labelsLength) {
        labelsLength++;
      }
      dotNum = 0;
      while (dotNum < labels.length) {
        x1 = width * dotNum * (dotCount / labelsLength) / (dotCount - 1) + offsetLeft;

        if (dotNum + 1 == labels.length) {
          ctx.textAlign = "right";
        }
        ctx.fillText(labels[dotNum], x1, height + 28);

        dotNum++;
      }
      ctx.textAlign = "left";
    }

    if (leftLabels) {
      ctx.font = "13px Arial";
      ctx.textBaseline = "middle";
      ctx.lineWidth = 0.5;

      dotNum = 0;
      var intervalsCount = 4;
      while (dotNum <= intervalsCount) {
        ctx.beginPath();
        ctx.moveTo(offsetLeft, dotNum * height / intervalsCount + offsetTop);
        ctx.lineTo(width + offsetLeft, dotNum * height / intervalsCount + offsetTop);

        ctx.stroke();

        var currentPrice = maxElement - (maxElement - minElement) / intervalsCount * dotNum;
        ctx.fillText("$ " + Math.round(currentPrice * 100) / 100
          , 5
          , dotNum * height / intervalsCount + offsetTop);

        dotNum++;
      }
    }
  }

  drawNoData(element) {
    var ctx = element.getContext("2d");
    ctx.clearRect(0, 0, element.width, element.height);

    ctx.font = "240px Montserrat";
    ctx.textBaseline = "top";
    ctx.textAlign = "left";
    ctx.fillStyle = 'white';

    ctx.fillText("NO DATA", 242, 8, 600);
  }
}

export { PriceGraphHelper };