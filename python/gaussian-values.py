from numpy.core.numeric import array_equal
from numpy.lib import math
import scipy.ndimage
import scipy.stats
from scipy import signal
import matplotlib.pyplot as plt
import numpy as np
np.set_printoptions(suppress=True)


def gauss1d_a(pointCount: int, std: float):
    return 


def gauss1d_b(pointCount: int, std: float):
    xs = range(pointCount)
    xs = [x - (pointCount - 1)/2 for x in xs]
    ys = np.exp(-np.square(xs)/(2 * std * std))
    return ys


def show1d():
    a = gauss1d_a(25, 3)
    b = gauss1d_b(25, 3)
    np.testing.assert_array_almost_equal(a, b)
    print(", ".join([str(x) for x in a]))
    plt.plot(a)
    plt.plot(b)
    plt.show()

def show2d():
    gkern1d = gauss1d_b(5, 2)
    gkern2d = np.outer(gkern1d, gkern1d)
    #plt.imshow(gkern2d, interpolation='none', cmap='Greys_r')
    #plt.show()
    print(gkern2d)


if __name__ == "__main__":
    show2d()